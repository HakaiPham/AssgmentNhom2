using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestNV2 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] _CarMachine;
    [SerializeField] private int _QuantityCarMachine;
    [SerializeField] GameObject currentObject;
    [SerializeField] private TextMeshProUGUI _QuantityText;
    [SerializeField] private Slider _SliderWaitFixCar;
    [SerializeField] private int _WaitFixCarProgress;
    [SerializeField] private GameObject _PanelProgress;
    private List<int> spawnedIndexes = new List<int>(); // Danh sách các index đã spawn
    private bool _HasTriggered = false;//Biến để đảm bảo chỉ va chạm 1 lần
    void Start()
    {
        StartCoroutine(Quest());
        _SliderWaitFixCar.maxValue = 60;
        _PanelProgress.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
    }
    public void Quest2()
    {
        //Người chơi sẽ phải thu thập các phụ kiện để sửa chữa xe
        //Xe sẽ xuất hiện trong trạng thái sửa chữa
        //Mỗi lần thu thập phụ kiện thì số phụ kiện sẽ tăng lên 1 (Max là 6)
        //Mỗi 30s sẽ xuất hiện 1 phụ kiện ngẫu nhiên và chỉ có 1 phụ kiện được xuất hiện trên bản đồ
        //Các phụ kiện đã thu thập sẽ không thể xuất hiện thêm lần nữa
        //Sau khi thu thập đủ phụ kiện xe sẽ trong trạng thái tu sửa tầm 1p và người chơi sẽ cầm cự cho đến lúc dó
        //Sau khi hoàn thành thì sẽ chiến thắng.
    }
    IEnumerator Quest()
    {
        // Biến để theo dõi object hiện tại

        while (true) // Lặp vô hạn để quản lý việc tạo object
        {
            if (_QuantityCarMachine == _CarMachine.Length) break; // Đã spawn hết tất cả item, dừng lại

            // Nếu chưa có object trong bản đồ, tạo object mới
            if (currentObject == null)
            {
                int randomIndex;

                // Tìm một index chưa được spawn
                do
                {
                    randomIndex = Random.Range(0, _CarMachine.Length);
                } while (spawnedIndexes.Contains(randomIndex)); // Lặp cho đến khi tìm được index chưa spawn

                spawnedIndexes.Add(randomIndex);

                // Chọn vị trí spawn ngẫu nhiên
                Vector3 spawnPosition = GenerateRandomPosition();

                // Tạo object từ mảng _CarMachine
                currentObject = Instantiate(_CarMachine[randomIndex], spawnPosition, Quaternion.identity);

                // Log thông báo
                Debug.Log("Created new object: " + currentObject.name);
            }
            while (currentObject != null)
            {
                yield return null; // Đợi 1 frame
            }

            // Thêm một chút delay trước khi tạo object tiếp theo (nếu cần)
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("All items have been spawned.");
    }
    private Vector3 GenerateRandomPosition()
    {
        int random = Random.Range(0, 4);
        float randomPosition = 0;

        switch (random)
        {
            case 0:
                randomPosition = Random.Range(436f, 489.8f);
                return new Vector3(randomPosition, 0.2f, 391.7f);
            case 1:
                randomPosition = Random.Range(359.9f, 324f);
                return new Vector3(400.7f, 0.2f, randomPosition);
            case 2:
                randomPosition = Random.Range(365.1f, 328.1f);
                return new Vector3(randomPosition, 0.2f, 391.7f);
            case 3:
                randomPosition = Random.Range(421.2f, 452.1f);
                return new Vector3(400.7f, 0.2f, randomPosition);
            default:
                Debug.LogError("Random value out of range: " + random);
                return Vector3.zero; // Giá trị mặc định
        }
    }
    public int MissionProgress()
    {
        _QuantityCarMachine += 1;
        _QuantityCarMachine= Mathf.Max(0,_QuantityCarMachine);
        _QuantityText.text = "- Thu Thập 6 phụ kiện xe: "+_QuantityCarMachine +" / 6";
        return _QuantityCarMachine;
    }
    IEnumerator WaitCarFixed()
    {
        if(_QuantityCarMachine == 6)
        {
            _QuantityText.color = Color.green;
            _PanelProgress.SetActive(true);
            while (true)
            {
                _WaitFixCarProgress+=1;
                _WaitFixCarProgress = Mathf.Max(0,_WaitFixCarProgress);
                _SliderWaitFixCar.value = _WaitFixCarProgress;
                yield return new WaitForSeconds(1f);
                if(_WaitFixCarProgress >= 60)
                {
                    break;
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_HasTriggered) return;
        if (collision.gameObject.CompareTag("Player")&& _QuantityCarMachine == 6)
        {
            _HasTriggered = true;
            Debug.Log("Đã va chạm");
            // Bắt đầu logic xử lý (không phụ thuộc việc thoát khỏi vùng va chạm).
            if (_QuantityCarMachine == 6)
            {
                StartCoroutine(WaitCarFixed());
            }
        }
    }
}
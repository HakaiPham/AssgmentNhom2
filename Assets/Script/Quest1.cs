using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quest1 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI _Mission1Text;
    [SerializeField] private int _QuantityHerb;
    [SerializeField] private GameObject[] _Herb;
    [SerializeField] private GameObject _CurrentObject = null;
    private List<int> spawnedIndexes = new List<int>(); // Danh sách các index đã spawn
    [SerializeField] private int _WaitItemSpawn = 15;
    [SerializeField] private TextMeshProUGUI _NoticeText; //Thông báo spawn sản phẩm
    [SerializeField]
    private GameObject _PanelText;
    [SerializeField]
    private GameObject _PanelNotice;
    [SerializeField] private QuestNV2 _QuestNV2;
    private bool _CheckMissionIsFinished = false;
    // Sự kiện thông báo hoàn thành nhiệm vụ
    void Start()
    {
        _QuestNV2.enabled = false;
        if (!_CheckMissionIsFinished)
        {
            StartCoroutine(Mission1());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_CheckMissionIsFinished)
        {
            _PanelText.SetActive(false);
            _QuestNV2.enabled = true;
        }
    }
    IEnumerator Mission1()
    {
        // Biến để theo dõi object hiện tại
        while (true) // Lặp vô hạn để quản lý việc tạo object
        {
            if (_QuantityHerb == _Herb.Length)
            {
                break;
            }

            // Nếu chưa có object trong bản đồ, tạo object mới
            if (_CurrentObject == null)
            {
                int randomIndex;

                // Tìm một index chưa được spawn
                do
                {
                    randomIndex = Random.Range(0, _Herb.Length);
                } while (spawnedIndexes.Contains(randomIndex)); // Lặp cho đến khi tìm được index chưa spawn

                spawnedIndexes.Add(randomIndex);

                // Chọn vị trí spawn ngẫu nhiên
                Vector3 spawnPosition = GenerateRandomPosition();

                // Tạo object từ mảng _CarMachine
                _CurrentObject = Instantiate(_Herb[randomIndex], spawnPosition, Quaternion.identity);

                // Log thông báo
                _PanelNotice.SetActive(true);
                _NoticeText.text = "Nguyên liệu thuốc đã xuất hiện: " + _CurrentObject.name;
            }
            while (_CurrentObject != null)
            {
                yield return null; // Đợi 1 frame
            }

            // Thêm một chút delay trước khi tạo object tiếp theo (nếu cần)
            // _PanelNotice.SetActive(false);
            _PanelNotice.SetActive(false);
            yield return new WaitForSeconds(_WaitItemSpawn);
        }
        Debug.Log("All items have been spawned.");
        // Khi nhiệm vụ hoàn thành
    }
    private Vector3 GenerateRandomPosition()
    {
        int random = Random.Range(0, 4);
        float randomPosition = 0;

        switch (random)
        {
            case 0:
                randomPosition = Random.Range(353f, 372.2f);
                return new Vector3(randomPosition, 0.87f, 394.3f);
            case 1:
                randomPosition = Random.Range(352.4f, 379.7f);
                return new Vector3(402.27f, 0.87f, randomPosition);
            case 2:
                randomPosition = Random.Range(431.4f, 442.22f);
                return new Vector3(randomPosition, 0.87f, 394.3f);
            case 3:
                randomPosition = Random.Range(426f, 447.4f);
                return new Vector3(400.5f, 0.87f, randomPosition);
            default:
                Debug.LogError("Random value out of range: " + random);
                return Vector3.zero; // Giá trị mặc định
        }
    }
    public int MissionProgress()
    {
        _QuantityHerb += 1;
        _QuantityHerb = Mathf.Max(0, _QuantityHerb);
        _Mission1Text.text = "- Thu thập thuốc: " + _QuantityHerb + " / 3";
        if (_QuantityHerb == 3)
        {
            _CheckMissionIsFinished = true;
            _Mission1Text.color = Color.green;
        }
        return _QuantityHerb;
    }
}


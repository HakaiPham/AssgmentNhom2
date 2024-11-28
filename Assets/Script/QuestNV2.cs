using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestNV2 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] _CarMachine;
    [SerializeField] private int _QuantityCarMachine;
    [SerializeField] GameObject currentObject;
    void Start()
    {
        StartCoroutine(Quest());
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CarMachine")
        {
            _QuantityCarMachine += 1;
            Destroy(other.gameObject);
        }
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
            // Nếu chưa có object trong bản đồ, tạo object mới
            if (currentObject == null)
            {
                // Chọn vị trí ngẫu nhiên
                // Tạo object từ mảng _CarMachine
                Vector3 spawnPosition = GenerateRandomPosition();
                int randomIndex = Random.Range(0, _CarMachine.Length);
                currentObject = Instantiate(_CarMachine[randomIndex], spawnPosition, Quaternion.identity);
                // Log thông báo
                Debug.Log("Created new object: " + currentObject.name);
            }

            // Chờ cho đến khi object hiện tại bị hủy
            while (currentObject != null)
            {
                yield return null; // Đợi 1 frame
            }
            // Thêm một chút delay trước khi tạo object tiếp theo (nếu cần)
            yield return new WaitForSeconds(1f);
        }
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
}
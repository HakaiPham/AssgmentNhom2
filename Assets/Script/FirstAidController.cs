using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _FirstAid;
    [SerializeField]
    private GameObject _CurrentObject=null;
    void Start()
    {
        if( _FirstAid != null)
        {
            StartCoroutine(FirstAidSpawn());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator FirstAidSpawn()
    {
        // Biến để theo dõi object hiện tại

        while (true) // Lặp vô hạn để quản lý việc tạo object
        {            // Nếu chưa có object trong bản đồ, tạo object mới
            if (_CurrentObject == null)
            {
                // Chọn vị trí spawn ngẫu nhiên
                Vector3 spawnPosition = GenerateRandomPosition();

                // Tạo object từ mảng _CarMachine
                _CurrentObject = Instantiate(_FirstAid, spawnPosition, Quaternion.identity);

            }
            while (_CurrentObject != null)
            {
                yield return null; // Đợi 1 frame
            }

            // Thêm một chút delay trước khi tạo object tiếp theo (nếu cần)
            yield return new WaitForSeconds(Random.Range(5,11));
            Debug.Log("Đã Spawn");
        }
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
}

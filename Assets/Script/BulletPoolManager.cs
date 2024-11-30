using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bulletPrefapt;
    [SerializeField] private int poolsize = 30;
    private Queue<GameObject> bulletpool;
    private void Awake()
    {
        bulletpool = new Queue<GameObject>();
        for(int i = 0; i < poolsize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefapt);
            bullet.SetActive(false);
            bulletpool.Enqueue(bullet);
        }
    }
    public GameObject GetBullet()
    {
        if(bulletpool.Count > 0)
        {
            GameObject bullet = bulletpool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            return null;
        }
    }
    public void ReturnBullet(GameObject bullet)
    {
        if (bullet == null)
        {
            Debug.LogError("Cannot return a null bullet to the pool.");
            return;
        }
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Reset vận tốc
            rb.angularVelocity = Vector3.zero; // Reset góc quay
        }

        bullet.transform.position = Vector3.zero; // Reset vị trí
        bullet.transform.rotation = Quaternion.identity; // Reset hướng
        bullet.SetActive(false); // Vô hiệu hóa đạn
        bulletpool.Enqueue(bullet); // Đưa đạn trở lại queue
    }
}

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
        //if (bullet == null)
        //{
        //    Debug.LogError("Cannot return a null bullet to the pool.");
        //    return;
        bullet.SetActive(false); // Đặt trạng thái đạn về tắt
        bulletpool.Enqueue(bullet); // Trả lại đạn vào pool
    }
}

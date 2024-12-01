using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class DemonAttack : MonoBehaviour
{
    public int damage;
    public string targetTag;

    
    
    
    [SerializeField] private Transform Whereshot;
    //Bullet Property
    [SerializeField] private GameObject Bullet; // Nhét Prefab viên đạn vào đây
    public float bulletSpeed; // Tốc đọ viên đạn
    public float killTime; // Thời gian tồn tại của viên đạn.
    
    // Start is called before the first frame update
    
    public void Fire()
    {
            GameObject shot = Instantiate(Bullet, Whereshot.transform.position, Quaternion.identity);
            shot.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            Destroy(shot, killTime);
    }
    
}

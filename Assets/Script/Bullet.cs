using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    Enemy _enemy;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    BulletPoolManager poolManager;
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        poolManager = FindObjectOfType<BulletPoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDame(Random.Range(20, 51));
            rb.isKinematic = true;
            capsuleCollider.enabled = false;
            gameObject.SetActive(false);
            poolManager.ReturnBullet(gameObject);
        }
    }
}

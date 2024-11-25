using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera _maincamera;
    [SerializeField] private Transform _tranformGun;
    [SerializeField] private GameObject _Bullet;
    Player player;
    [SerializeField] private float _speedBullet;
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerCooldownShoot = player.PlayerCooldownShooter();
        Debug.Log("TimeCooldown: " + playerCooldownShoot);
        if (Input.GetMouseButtonDown(0)&&playerCooldownShoot<=0) 
        {
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Huong ban
                Vector3 target = hit.point;
                Vector3 direction = (target - _tranformGun.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);//Xoay Player theo hướng bắn
                PlayerShoot(direction);
            }
        }
    }
    public void PlayerShoot(Vector3 direction)
    {
        Debug.Log("Firing towards: " + direction);
        GameObject bullet = Instantiate(_Bullet, _tranformGun.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = direction*_speedBullet;
        Destroy(bullet, 3f);
    }
}

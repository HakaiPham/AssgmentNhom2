using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera _maincamera;
    [SerializeField] private Transform _tranformGun;
    [SerializeField] private GameObject _Bullet;
    Player player;
    [SerializeField] private float _speedBullet;
    [SerializeField] private TextMeshProUGUI _BulletBoxText;
    [SerializeField] private TextMeshProUGUI _GunReloadText;
    [SerializeField] private Image _GunImage;
    [SerializeField]
    private int bulletInBox = 30;
    [SerializeField] private int _TimeGunReload;
    bool CanReload;
    [SerializeField] private GameObject _CooldownGunPanel;
    bool _isReloading = false;
    void Start()
    {
        player = GetComponent<Player>();
        _CooldownGunPanel.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerShooter();
    }
    public void PlayerShooter()
    {
        float playerCooldownShoot = player.PlayerCooldownShooter();
        Debug.Log("Thời gian cooldown Script1 còn: " + player.PlayerCooldownShooter()
            + "Thời gian cooldown Script2 còn: " + playerCooldownShoot);
        //  bool pressMouse = Input.GetMouseButtonDown(0);
        // Debug.Log("PressMouse?: " + pressMouse);
        bool isDead = player.PlayerIsDead();
        if (Input.GetMouseButtonDown(0) && bulletInBox != 0)
        {
            if (isDead) return;
            Debug.Log("TMDK");
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Huong ban
                Vector3 target = hit.point;
                Vector3 direction = (target - _tranformGun.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);//Xoay Player theo hướng bắn
                if(playerCooldownShoot <= 0)
                {
                    CreateBullet(direction);
                }
            }
        }
        else
        {
            Debug.Log("KTMDK");
        }
        //Tránh cho Hàm Update gọi Coroutine liên tục
        if (Input.GetMouseButton(0) && bulletInBox > 0 && !_isReloading)
        {
            if (isDead) return;
            StartCoroutine(GunBullet());
        }
        else if ((bulletInBox == 0 || Input.GetKeyDown(KeyCode.R)) && !_isReloading)
        {
            if (isDead) return;
            StartCoroutine(GunBullet());
        }
    }
    public void CreateBullet(Vector3 direction)
    {
        Debug.Log("Firing towards: " + direction);
        GameObject bullet = Instantiate(_Bullet, _tranformGun.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = direction*_speedBullet;
        Destroy(bullet, 3f);
    }
    IEnumerator GunBullet()
    {
        //Đầu tiên sẽ có 30 viên đạn trong băng
        //Sau khi bắn sẽ trừ số đạn còn lại
        //Sau khi bắn hết đạn trong băng sẽ chạy thời gian reload đạn
        //Súng sẽ chuyển sang màu đen
        if (Input.GetMouseButton(0) && bulletInBox != 0)
        {
            if (bulletInBox == 0||_isReloading) yield break;
            bulletInBox--;
            bulletInBox = Mathf.Max(0, bulletInBox);
            _BulletBoxText.text = "" + bulletInBox+"/" + "∞";
            yield return new WaitForSeconds(1f); //Chờ 1 frame để không bị gọi liên tục
        }
        else if ((bulletInBox == 0 || Input.GetKeyDown(KeyCode.R))&&!_isReloading)
        {
            if (bulletInBox >= 30)
            {
                Debug.Log("Đạn đã đầy.");
                yield break;
            }
            _isReloading = true;
            _CooldownGunPanel.SetActive(true);
            _GunImage.color = Color.black;
            for (int i = 0; i < 5; i++) // Ví dụ reload trong 3 giây
            {
                _TimeGunReload--;
                _TimeGunReload = Mathf.Max(0, _TimeGunReload);
                _GunReloadText.text = "" + _TimeGunReload;
                yield return new WaitForSeconds(1f); // Chờ 1 giây mỗi lần
            }
            if (_TimeGunReload == 0)
            {
                _TimeGunReload = 5;//Đặt lại thời gian
                bulletInBox = 30;
                _BulletBoxText.text = "" + bulletInBox + "/" + "∞";
                _CooldownGunPanel.SetActive(false);
                _GunImage.color = Color.white;
                _isReloading = false;
            }
        }
    }
}

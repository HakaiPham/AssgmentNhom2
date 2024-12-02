using JetBrains.Annotations;
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
    [SerializeField] private float _speedBullet;
    [SerializeField] private TextMeshProUGUI _BulletBoxText;
    [SerializeField] private TextMeshProUGUI _GunReloadText;
    [SerializeField] private Image _GunImage;
    [SerializeField]
    private int bulletInBox = 30;
    [SerializeField] private int _TimeGunReload;//Thời gian reload đạn
    [SerializeField] private GameObject _CooldownGunPanel;
    [SerializeField] private float _WaitFireBullet;
    public Coroutine _Coroutine;
    [SerializeField] private BulletPoolManager _BulletPoolManager;
    Player player;
    bool _isReloading = false;
    void Start()
    {
        _CooldownGunPanel.SetActive(false);
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerShooter();
    }
    public void PlayerShooter()
    {
        bool isDead = player.PlayerIsDead();
        if (Input.GetMouseButtonDown(0) && bulletInBox != 0)
        {
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Huong ban
                Vector3 target = hit.point;
                Vector3 direction = (target - _tranformGun.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);//Xoay Player theo hướng bắn
                bool playerCooldownShoot = player.CanAttack();
                if (playerCooldownShoot&&!_isReloading)
                {
                    CreateBullet(direction);
                }
            }
        }
        if(!isDead)
        {
            StartCoroutine(GunBullet());
        }
    }
    public void CreateBullet(Vector3 direction)
    {
        GameObject bullet = _BulletPoolManager.GetBullet();
        if (bullet != null)
        {
            bullet.transform.position = _tranformGun.position;
            bullet.transform.rotation = Quaternion.identity;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * _speedBullet;
                Debug.Log("Bullet velocity: " + rb.velocity);
            }
            else
            {
                Debug.LogWarning("No Rigidbody found on bullet!");
            }
            StartCoroutine(DeactivateBulletAfterTime(bullet, 3f));
        }
        else
        {
            Debug.LogWarning("No bullets available in pool!");
        }
    }

    IEnumerator DeactivateBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        _BulletPoolManager.ReturnBullet(bullet);
    }
    IEnumerator GunBullet()
    {
        //Đầu tiên sẽ có 30 viên đạn trong băng
        //Sau khi bắn sẽ trừ số đạn còn lại
        //Sau khi bắn hết đạn trong băng sẽ chạy thời gian reload đạn
        //Súng sẽ chuyển sang màu đen
        if (bulletInBox <= 0||Input.GetKeyDown(KeyCode.R))
        {
            if (_Coroutine == null)
            {
                _Coroutine = StartCoroutine(ReloadBullet());
                yield break;
            }
            else
            {
                yield break;
            }
        }
        if (Input.GetMouseButtonDown(0)&&!_isReloading)
        {
            bulletInBox--;
            bulletInBox = Mathf.Max(0, bulletInBox);
            _BulletBoxText.text = "" + bulletInBox + "/" + "∞";
            yield return new WaitForSeconds(_WaitFireBullet); //Chờ 1 frame để không bị gọi liên tục
        }
    }
    IEnumerator ReloadBullet()
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
            yield return new WaitForSeconds(1); // Chờ 1 giây mỗi lần
        }
        if (_TimeGunReload == 0)
        {
            _TimeGunReload = 5;//Đặt lại thời gian
            bulletInBox = 30;
            _BulletBoxText.text = "" + bulletInBox + "/" + "∞";
            _CooldownGunPanel.SetActive(false);
            _GunImage.color = Color.white;
            _Coroutine = null;
            _isReloading = false;
        }
    }
    public bool CheckGunIsReload()
    {
        return _isReloading;
    }
}

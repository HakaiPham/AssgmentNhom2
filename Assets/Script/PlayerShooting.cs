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
    bool _isReloading = false;
    bool _CheckHitCollier = false;
    bool canshoot = false;
    public AudioClip _ReloadGunSound;
    public AudioSource _AudioSource;
    void Start()
    {
        _CooldownGunPanel.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!canshoot)
        {
            PlayerShooter();
        }
    }
    public void PlayerShooter()
    {
        StartCoroutine(GunBullet());
        if (bulletInBox <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            if (_Coroutine == null)
            {
                _Coroutine = StartCoroutine(ReloadBullet());
            }
        }
    }


    IEnumerator GunBullet()
    {
        //Đầu tiên sẽ có 30 viên đạn trong băng
        //Sau khi bắn sẽ trừ số đạn còn lại
        //Sau khi bắn hết đạn trong băng sẽ chạy thời gian reload đạn
        //Súng sẽ chuyển sang màu đen
        if (Input.GetMouseButtonDown(0)&&!_isReloading)
        {
            canshoot = true;
            bulletInBox--;
            bulletInBox = Mathf.Max(0, bulletInBox);
            _BulletBoxText.text = "" + bulletInBox + "/" + "∞";
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Đặt giá trị mặc định cho CheckHitCollider
                Vector3 target = hit.point;
                Vector3 direction = (target - _tranformGun.position).normalized;
                direction.y = 0f;//Giữ nhân vật không xoay theo trục y
                transform.rotation = Quaternion.LookRotation(direction);
                _CheckHitCollier = hit.collider.CompareTag("Enemy");
                //  xoay nhân vật
                if (_CheckHitCollier)
                {
                    //  xoay nhân vậ
                    // Gây sát thương cho Enemy
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    enemy?.TakeDame(Random.Range(20, 80)); // Gây sát thương ngẫu nhiên

                }
            }
            yield return new WaitForSeconds(_WaitFireBullet); //Chờ 1 frame để không bị gọi liên tục
            canshoot = false;
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
        _AudioSource.PlayOneShot(_ReloadGunSound);
        for (int i = 0; i < 3; i++) // Ví dụ reload trong 3 giây
        {
            _TimeGunReload--;
            _TimeGunReload = Mathf.Max(0, _TimeGunReload);
            _GunReloadText.text = "" + _TimeGunReload;
            yield return new WaitForSeconds(1); // Chờ 1 giây mỗi lần
        }
        if (_TimeGunReload == 0)
        {
            _TimeGunReload = 3;//Đặt lại thời gian
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
    public bool CheckHitColliderEnemy()
    {
        return _CheckHitCollier;
    }
}

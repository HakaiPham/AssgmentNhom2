using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Animator animator;
    private Vector3 movement;
    [SerializeField] private CharacterController characterController;
    bool isAttack=false;
    AnimatorStateInfo stateInfo;
    [SerializeField] private int _MaxHp;
    [SerializeField] private int _CurrentHp;
    bool isDead = false;
    [SerializeField] private GameObject _PanelDead;
    [SerializeField] private Slider _SliderHealth;
    public QuestNV2 _QuestNV2;
    public ReloadBulletManager _ReloadBulletManager;
    PlayerShooting playerShooting;
    [SerializeField] private Quest1 _Quest1;
    AudioSource _AudioSource;
    public AudioClip _SoundGun;
    public AudioClip _SoundWalk;
    bool _isStartSound = false;
    [SerializeField] private GameObject _VFXGunFire;
    public TimeLineStory _timeLineStory; 
    void Start()
    {
        
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        _CurrentHp = _MaxHp;
        _SliderHealth.maxValue = _MaxHp;
        playerShooting = GetComponent<PlayerShooting>();
        _AudioSource = GetComponent<AudioSource>();
        _VFXGunFire.SetActive(false);

    }

    private void Update()
    {
        bool checkTimeLine = _timeLineStory.CheckTimeLineStart();
        if (checkTimeLine)
        {

            return;
        }
        if (currentState == CharacterState.Dead) return;


        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (currentState == CharacterState.Normal)
        {
            MoveSpeed();
        }
        else if (currentState == CharacterState.Attack)
        {
            // Khóa chuyển động trong trạng thái tấn công
            // Kiểm tra trạng thái Animation Attack
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Attack")&&stateInfo.normalizedTime>=1)
            {
                EndAttack();
            }
        }

        // Di chuyển nhân vật
        characterController.Move(movement * Time.deltaTime);
    }
    public void Attack()
    {
        // Kiểm tra tất cả điều kiện cần thiết để bắn
        bool canReload = _ReloadBulletManager.CheckDKReload();
        bool gunIsReloaded = playerShooting.CheckGunIsReload();
        Debug.Log("Can Reload: " + canReload);
        if (canReload)
        {
            Debug.Log("TMDK");
            isAttack = true;
            Debug.Log("Gun Is Reload: " + gunIsReloaded);
            if (!gunIsReloaded)
            {
                _VFXGunFire.SetActive(true);
                _AudioSource.PlayOneShot(_SoundGun);
                StartCoroutine(OffVFX());
            }
            ChangeState(CharacterState.Attack);
            _ReloadBulletManager.ResetTimeReload();
        }
    }
    IEnumerator OffVFX()
    {
        yield return new WaitForSeconds(0.5f);
        _VFXGunFire.SetActive(false);
    }
    public enum CharacterState 
    {
       Normal,Attack,Dead
    }
    public CharacterState currentState;
    private void MoveSpeed()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        if (currentState != CharacterState.Normal)
        {
            Debug.Log("Finished!");
            return;
        }
        // Tính toán hướng di chuyển theo camera
        Camera mainCamera = Camera.main;//(Lấy MainCamera)
        Vector3 forward = mainCamera.transform.forward;//Mặc định là trục Z
        //(Lấy hướng(Z và X) của Camera đang nhìn để tiến hành di chuyển)
        Vector3 right =  mainCamera.transform.right;//Mặc định là trục  X
        forward.y = 0;//Chỉ di chuyển trên mặc đất không ảnh hướng đến chiều cao
        right.y = 0;

        movement = (forward * verticalInput + right * horizontalInput).normalized * speed;

        bool _CheckHitCollider = playerShooting.CheckHitColliderEnemy();
        // Xoay hướng nhân vật
        if (movement.magnitude > 0 && !Input.GetMouseButtonDown(0))//ưu tiên việc xoay nhân vật khi nhân vật di chuyển
            //không thì xoay theo hướng bắn
        {
            Quaternion toRotation = Quaternion.LookRotation(movement);
            transform.rotation = toRotation;
            animator.SetBool("Run", true);
            if (!_AudioSource.isPlaying)
            {
                _AudioSource.PlayOneShot(_SoundWalk);
            }
        }
        else if(movement.magnitude == 0)
        {
            _AudioSource.Stop();
            animator.SetBool("Run", false);
        }
    }
    public void ChangeState(CharacterState newState)
    {
        switch (newState)
        {
            case CharacterState.Normal:break;
            case CharacterState.Attack:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    animator.ResetTrigger("EndAttack");
                }
                animator.SetTrigger("Attack");
                break;
            case CharacterState.Dead: break;
        }
        currentState = newState;
    }
    public void EndAttack()
    {
        animator.SetTrigger("EndAttack");
        isAttack = false;
        ChangeState(CharacterState.Normal);
    }
    public void TakeDame(int dame)
    {
        _CurrentHp -= dame;
        _CurrentHp = Mathf.Max(0, _CurrentHp);
        _SliderHealth.value = _CurrentHp;
        if(_CurrentHp <= 0)
        {
            isDead = true;
            ChangeState(CharacterState.Dead);
            _PanelDead.SetActive(true);
            Debug.Log("Character Die");
        }
    }
    public bool PlayerIsDead()
    {
        return isDead;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CarMachine")
        {
            _QuestNV2.MissionProgress();
            Destroy(other.gameObject);
        }
        else if(other.tag == "Herb")
        {
            _Quest1.MissionProgress();
            Destroy(other.gameObject);
        }
        if(other.tag == "FirstAid")
        {
            Debug.Log("Đã va chạm");
            _CurrentHp += 20;
            _CurrentHp = Mathf.Min(_CurrentHp, 100);
            _SliderHealth.value = _CurrentHp;
            Destroy(other.gameObject);
        }
    }
    public bool CanAttack()
    {
        return isAttack;
    }
}

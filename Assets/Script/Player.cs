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
    [SerializeField] private GameObject _VFXGunFire;
    PlayerShooting playerShooting;
    [SerializeField] private Quest1 _Quest1;
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        _CurrentHp = _MaxHp;
        _SliderHealth.maxValue = _MaxHp;
        _VFXGunFire.SetActive(false);
        playerShooting = GetComponent<PlayerShooting>();
    }

    private void Update()
    {
        if (currentState == CharacterState.Dead) return;
        if (!isAttack&&Input.GetMouseButtonDown(0))
        {
            bool reloadBulletScript = _ReloadBulletManager.CheckDKReload();
            bool checkGunIsLoad = playerShooting.CheckGunIsReload();
            if (reloadBulletScript)
            {
                Debug.Log("Đã khởi chạy");
                isAttack = true;
                if (!checkGunIsLoad)
                {
                    _VFXGunFire.SetActive(true);
                }
                ChangeState(CharacterState.Attack);
                _ReloadBulletManager.ResetTimeReload();
            }
        }
       if(currentState == CharacterState.Normal) { 
            MoveSpeed();
       }
       else if(currentState == CharacterState.Attack)
       {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            // Lấy thông tin clip đang chạy trong state
            AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
            Debug.Log("Animation đã lấy là: "+clipInfos[0].clip.name);
            if (stateInfo.IsName("Attack"))
            {
                EndAttack();
            }
       }
        characterController.Move(movement*Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDame(10);
        }
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
        if (currentState!=CharacterState.Normal)
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


        // Xoay hướng nhân vật
        if (movement.magnitude > 0)
        {
            animator.SetBool("Run", true);
            transform.rotation = Quaternion.LookRotation(movement);
        }
        else if(movement.magnitude == 0)
        {
            animator.SetBool("Run", false);
        }
    }
    public void ChangeState(CharacterState newState)
    {
        switch (newState)
        {
            case CharacterState.Normal:break;
            case CharacterState.Attack:
                animator.SetBool("Attack",true);
                break;
            case CharacterState.Dead: break;
        }
        currentState = newState;
    }
    public void EndAttack()
    {
        animator.SetBool("Attack", false);
        isAttack = false;
        _VFXGunFire.SetActive(false);
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

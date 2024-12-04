using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
     // Entity essential
    private NavMeshAgent agent;
    private Animator animator;
    public SpawnEnemy spawnEnemySystem;
    
    
    // SerializeField
    [SerializeField] Transform target;

    // Entity property
    //public
    public int curHP;
    public int maxHP;
    public float chaseDistance;
    public float maxChaseDistance;
    public float attackDistance;
    private float cooldownTimer;
    public bool isZombieSuper;
    //private
    
    private bool isAttacking;
    public float cooldown;
    public ParticleSystem _HurtEffect;
    [SerializeField] private AttackZone _Attackzone;
    bool _IsHitDame = false;
    Rigidbody _Rigidbody;
    CapsuleCollider _CapsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        // GetComponent
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _CapsuleCollider = GetComponent<CapsuleCollider>();
        _Rigidbody = GetComponent<Rigidbody>();
        // Reset giá trị
        curHP = maxHP;
        isAttacking = false;
        cooldownTimer = cooldown;
        target = FindObjectOfType<Player>().transform;
        spawnEnemySystem = FindObjectOfType<SpawnEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (curState == EnemyState.Dead) return;
        // Tính khoản cách giữa Enemy và Player
        float distance = Vector3.Distance(transform.position, target.position);
        // Duổi theo PLayer trong phạm vi cho phép
        if(distance > attackDistance)
        {
            Debug.Log("Can't Attack");
            agent.SetDestination(target.position);
            ChangeState(EnemyState.Run);
        }
        // Quay về vị trí cũ khi Player ở quá xa
        // Tấn công Player khi trong phạm vi tấn công
        if (distance <= attackDistance) 
        {
            Debug.Log("Can Attack");
            ChangeState(EnemyState.Attack);
        }
        gameObject.transform.LookAt(agent.destination);
    }
    
    private enum EnemyState
    {
        Normal, Run, Attack, Dead,
    }

    private EnemyState curState;

    // Chuyển state hành vi
    private void ChangeState(EnemyState newState)
    {
        switch (curState)
        {
            case EnemyState.Normal:
                break;
            
            case EnemyState.Run:
                break;
            
            case EnemyState.Attack: 
                break;
            
            case EnemyState.Dead:
                break;
            
        }

        switch (newState)
        {
            case EnemyState.Normal:
                
                break;
            
            case EnemyState.Run:
                animator.SetBool("Attack", false);
                break;
            
            case EnemyState.Attack:
                if (cooldownTimer >= cooldown && isAttacking == false) StartCoroutine(Shot());
                break;
            case EnemyState.Dead:
                animator.SetTrigger("Dead");
                break;

        }
        curState = newState;
    }
    
    IEnumerator Shot()
    {
        if (cooldownTimer > cooldown && isAttacking == false)
        {
            Debug.Log("TMDK");
            isAttacking = true;
            animator.SetBool("Attack", true);
            cooldownTimer = 0f;
            yield return new WaitForSeconds(cooldown);
        }
        isAttacking = false;
        yield return null;
    }
    public void TakeDame(int dame)
    {
        curHP -= dame;
        if (!_IsHitDame&&!_HurtEffect.isPlaying)
        {
            _HurtEffect.Play();
            _IsHitDame = true;
        }
        curHP = Mathf.Max(0, curHP);
        if (curHP <= 0f) {
            spawnEnemySystem.reduceListEnemyWhenEnemyDead(gameObject);
            _Rigidbody.velocity = Vector3.zero;
            _CapsuleCollider.enabled = false;
            if (!isZombieSuper) {
                gameObject.SetActive(false);
            }
            ChangeState(EnemyState.Dead);
            Destroy(gameObject,1.5f);
        }
        _IsHitDame = false;
    }
    public void BeginDame()
    {
        _Attackzone.beginDamage();
    }
    public void EndDame()
    {
        _Attackzone.endDamage();
    }
    public bool CheckEnemyCanAttack()
    {
        return isAttacking;
    }
}


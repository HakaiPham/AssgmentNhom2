using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
     // Entity essential
    private NavMeshAgent agent;
    private Animator animator;
    private DemonAttack demonAttack;
    private Vector3 originalPosition;
    
    
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
    //private
    
    private bool isAttacking;
    public float cooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        // GetComponent
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        demonAttack = GetComponent<DemonAttack>();
        
        // Reset giá trị
        curHP = maxHP;
        originalPosition = transform.position;
        isAttacking = false;
        cooldownTimer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (curState == EnemyState.Dead) return;
        // Tính khoản cách giữa Enemy và Player
        float distance = Vector3.Distance(transform.position, target.position);
        var chasedDistance = Vector3.Distance(transform.position, originalPosition);
        Debug.unityLogger.Log(string.Format("Distance: {0}", distance));
        
        if (chasedDistance <= chaseDistance && distance >= maxChaseDistance)
        {
            agent.SetDestination(originalPosition);
            ChangeState(EnemyState.Run);
        }

        if (distance > chaseDistance || chasedDistance > maxChaseDistance)
        {
            agent.SetDestination(originalPosition);
            ChangeState(EnemyState.Run);
        }

        

        // Duổi theo PLayer trong phạm vi cho phép
        if (distance <= chaseDistance)
        {
            
            //transform.localRotation = Quaternion.Euler(0f,transform.localRotation.y,transform.localRotation.z);
            agent.SetDestination(target.position);
            ChangeState(EnemyState.Run);
        }
        
        // Quay về vị trí cũ khi Player ở quá xa
        if (distance > maxChaseDistance)
        {
            agent.SetDestination(originalPosition);
            ChangeState(EnemyState.Run);
        }

        // Tấn công Player khi trong phạm vi tấn công
        if (distance <= attackDistance) 
        {
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
                Run();
                break;
            
            case EnemyState.Attack:
                if (cooldownTimer >= cooldown && isAttacking == false) StartCoroutine(Shot());
                break;
            
            case EnemyState.Dead: 
                 Dead();
                break;
        }
        curState = newState;
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private void Run()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    private void Dead()
    {
        animator.SetTrigger("Dead");
    }

    public void EndAttack()
    {
        
        isAttacking = false;
        animator.SetTrigger("EndAttack");
    }
    
    IEnumerator Shot()
    {
        if (cooldownTimer > cooldown && isAttacking == false)
        {
            isAttacking = true;
            Attack();
            cooldownTimer = 0f;
            yield return new WaitForSeconds(cooldown);
        }
        isAttacking = false;
        yield return null;
    }
    
}


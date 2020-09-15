using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed;
    private GameObject currentTarget;
    private float attackTimer;
    public Animator anim;
    private Rigidbody rigid;
    private NavMeshAgent navAgent;
    public UnitAttackStats unitAttackStats;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.fixedDeltaTime;

        Vector3 movement = navAgent.velocity;

        anim.SetFloat("Move", Mathf.Abs(movement.magnitude));

        if (currentTarget != null && GetComponent<UnitCombatController>().health >0)
        {
            navAgent.destination = currentTarget.transform.position;
            navAgent.stoppingDistance = unitAttackStats.attackRange;

            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (distance <= unitAttackStats.attackRange)
            {

                Attack();
            }
        }
    }
    public void ChaseUnit(GameObject target)
    {
        currentTarget = target;
    }
    public UnitCombatController GetCurrentTargetCombat()
    {
        return currentTarget.GetComponent<UnitCombatController>();
    }
    public void Attack()
    {
        if (attackTimer >= unitAttackStats.attackSpeed)
        {
            Debug.Log("Attacking Player");
            anim.SetTrigger("Attacking");
            //GameController.UnitTakeDamage(this, currentTarget.GetComponent<UnitController>());
            attackTimer = 0;
        }
    }
    
}

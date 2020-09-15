using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private GameObject currentTarget;
    public float stoppingDistance;
    private float attackTimer;
    public UnitAttackStats unitAttackStats;
    public UnitWorkStats unitWorkStats;
    [Header("Job")]
    public Job currentJob;
    public float workTimer;
    public float energy;
    public bool working;
    public enum Job { Farmer, Chef, Guard }
    

    public void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        attackTimer = 0;
        workTimer = 0;
        energy = unitWorkStats.energy;
    }
    public void Update()
    {
        //attackTimer += Time.fixedDeltaTime;
        //if (working)
        //{
        //    workTimer += Time.fixedDeltaTime;
        //}
        //if(currentTarget != null)
        //{
        //    navAgent.destination = currentTarget.transform.position;
        //    navAgent.stoppingDistance = unitAttackStats.attackRange;
        //
        //    float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        //    if (distance <= unitAttackStats.attackRange)
        //    {
        //  
        //        Attack();
        //    }
        //}
        //else
        //{
        //    navAgent.stoppingDistance = stoppingDistance;
        //}
    }


    //ANDA ATÉ UM LOCAL DETERMINADO
    public void MoveUnit(Vector3 dest)
    {
        navAgent.destination = dest;
        currentTarget = null;
    }

    public void SetJob(Vector3 dest, string job)
    {
        Debug.Log("Job: " + job);
        MoveUnit(dest);
        switch (job)
        {
            case "Farmer":
                currentJob = Job.Farmer;
                break;
            case "Guard":
                currentJob = Job.Guard;
                break;
            case "Chef":
                currentJob = Job.Chef;
                break;
        }
    }

    public void Work()
    {
        if (!working)
        {
            working = true;
        }
        if(energy >= 0)
        {
            if (workTimer >= unitWorkStats.workSpeed)
            {
                switch (currentJob)
                {
                    case Job.Farmer:
                        GameObject.Find("GameManager").GetComponent<GameController>().AddFood(unitWorkStats.carryingCapacity);
                        break;
                    case Job.Chef:
                        GameObject.Find("GameManager").GetComponent<GameController>().AddFood(unitWorkStats.carryingCapacity);
                        break;
                    case Job.Guard:
                        GameObject.Find("GameManager").GetComponent<GameController>().AddFood(unitWorkStats.carryingCapacity);
                        break;
                }
                workTimer = 0;
                energy -= 1;
            }
        }
        
    }


    //É DEFINIDA COMO SELECIONADA
    public void SetSelected(bool isSelected)
    {
        transform.Find("Highlight").gameObject.SetActive(isSelected);
    }


    //SEGUE(E ATACA POR ENQUANTO) UMA UNIDADE
    public void FollowUnit(GameObject target)
    {
        currentTarget = target;
    }


    //ATACA OUTRA UNIDADE
    //public void Attack()
    //{
    //    if(attackTimer >= unitAttackStats.attackSpeed)
    //    {
    //
    //        GameController.UnitTakeDamage(this,currentTarget.GetComponent<EnemyController>());
    //        attackTimer = 0;
    //    }
    //}


    //TOMA DANO DO INIMIGO
    public void TakeDamage(UnitController enemy, float damage)
    {
        StartCoroutine(Flasher(GetComponent<Renderer>().material.color));
    }
    IEnumerator Flasher(Color defaultColor)
    {
        Renderer render = GetComponent<Renderer>();
        for(int i = 0; i < 2; i++)
        {
            render.material.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            render.material.color = defaultColor;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Farm"))
        {
            
            Work();
        }
    }
}

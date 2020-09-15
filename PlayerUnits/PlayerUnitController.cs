using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// using UnityEngine.UI;

public class PlayerUnitController : MonoBehaviour
{
    private NavMeshAgent navAgent;
    public Animator anim;
    private GameObject currentTarget;
    public GameObject currentWorkStation;
    public float stoppingDistance;
    private float attackTimer;
    public UnitAttackStats unitAttackStats;
    public UnitWorkStats unitWorkStats;

    [Header("Job")]
    public Job lastJob;
    public Job currentJob;
    public float workTimer;
    public float energy;
    public bool working;
    public GameObject[] Farms;
    public GameObject[] Towers;
    public GameObject[] Kitchens;
    public GameObject[] Warehouses;
    public enum Job { Generalist, Farmer, Chef, Guard }
    public enum AIState { GoingToWork, Working, OnBreak, GoingToBed, Sleeping };

    [Header("AI Behaviour")]
    public AIState State;

    [Header("Housing")]
    public GameObject House;

    [Header("Basic Stats")]
    public bool Hungry;
    public bool Thirsty;
    public bool Ill;
    private float EnergyRecoverRate = 1f;

    [Header("Alert Status Icons")]
    public GameObject HungryIcon;
    public GameObject ThirstyIcon;
    public GameObject IllIcon;
    public GameObject HomelessIcon;


    public void Start()
    {
        lastJob = currentJob;
        navAgent = GetComponent<NavMeshAgent>();
        attackTimer = 0;
        workTimer = 0;
        energy = unitWorkStats.energy;
        Farms = GameObject.FindGameObjectsWithTag("Farm");
        Towers = GameObject.FindGameObjectsWithTag("Tower");
        Warehouses = GameObject.FindGameObjectsWithTag("Warehouse");
        navAgent.stoppingDistance = stoppingDistance;
    }
    public void Update()
    {
        UpdateStates();
        GoToJob();
        GoToSleep();
        UpdateAnimation();
        CheckAlertStatus();
        CheckAnyConditionAffectingEnergyRecovery();
        CheckHouseAvaliability();

        if(!currentWorkStation && currentJob == Job.Generalist){
            SearchForNewResource();
        }

        attackTimer += Time.fixedDeltaTime;
        if (State.Equals(AIState.Working))
        {
            workTimer += Time.fixedDeltaTime;
        }
        if (currentWorkStation != null)
        {
            navAgent.destination = currentWorkStation.transform.position;
        }

    }

    public void CheckHouseAvaliability(){
        if(!House){

            foreach (GameObject house in GameObject.FindGameObjectsWithTag("House"))
            {
                // print( gameObject.name +" - Qtd Casas: " + GameObject.FindGameObjectsWithTag("House").Length);
                if (!house.GetComponent<HouseController>().full)
                {
                    if(house.GetComponent<HouseController>().AddPerson()){
                        House = house;
                        HomelessIcon.SetActive(false);
                    }else{
                        // print("Lotou");
                    }
                }else{
                    // print("Ta Cheio");
                }
            }
            if(!House){
                HomelessIcon.SetActive(true);
            }
        }
    }

    public void UpdateAnimation()
    {
        anim.SetFloat("Move", Mathf.Abs(navAgent.velocity.magnitude));
        if (working)
        {
            
        }
    }
    public void GoToSleep()
    {
        if (State == AIState.GoingToBed && (currentWorkStation.GetComponent<HouseController>() == null || !currentWorkStation))
        {
                // print("Tenho casa, vou dormir");
                currentWorkStation = House;
                MoveUnit(currentWorkStation.transform.position);
            if(House){
            }
        }
    }

    public void UpdateStates()
    {
        if (energy == 100 && State == AIState.Sleeping)
        {
            State = AIState.GoingToWork;
        }
        if (energy == 50 && State == AIState.Working)
        {
            StartCoroutine(Break());
            State = AIState.OnBreak;
            working = false;
            
            anim.SetTrigger("Idle");
        }
        if (energy < 0)
        {
            energy = 0;
            if(State == AIState.Working)
            {
                EndShift();
            }
            State = AIState.GoingToBed;
            // print("Hora de dormir");
        }
        if (CheckForJobChanges())
        {
            if (energy >= 100 && State != AIState.Sleeping)
            {
                State = AIState.GoingToWork;
            }
        }
    }
    public IEnumerator Break()
    {
        
        yield return new WaitForSeconds(5);
        State = AIState.Working;
        energy = 40;
        Work();
    }
    public bool CheckForJobChanges()
    {
        if(lastJob != currentJob)
        {
            lastJob = currentJob;
            return true;
        }
        return false;
    }
    public void MoveUnit(Vector3 dest)
    {
        navAgent.destination = dest;
        currentTarget = null;
    }
    public void SetJob(string job)
    {
        Debug.Log("Job: " + job);
        //MoveUnit(dest);
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
            case "Generalist":
                currentJob = Job.Generalist;
                break;
        }
    }

    public void GoToJob()
    {
        if (State == AIState.GoingToWork)
        {
            switch (currentJob)
            {
                case Job.Farmer:
                    foreach (GameObject farm in Farms)
                    {
                        if (!farm.GetComponent<WorkPlaceController>().busy)
                        {
                            currentWorkStation = farm;
                            MoveUnit(currentWorkStation.transform.position);
                            farm.GetComponent<WorkPlaceController>().SetWorker(this);
                        }
                    }
                    break;
                case Job.Guard:
                    foreach (GameObject tower in Towers)
                    {
                        if (!tower.GetComponent<WorkPlaceController>().busy)
                        {
                            currentWorkStation = tower;
                            MoveUnit(currentWorkStation.transform.position);
                            tower.GetComponent<WorkPlaceController>().SetWorker(this);
                        }
                    }
                    break;
                case Job.Chef:
                    //currentJob = Job.Chef;
                    break;
                case Job.Generalist:
                    foreach (GameObject warehouse in Warehouses)
                    {
                        // if (!warehouse.GetComponent<WorkPlaceController>().busy)
                        // {
                            currentWorkStation = warehouse;

                            if(currentWorkStation.GetComponent<WarehouseController>()){
                                var distance = 1000000f;
                                GameObject selectedResource;
                                Warehouses[0].GetComponent<WarehouseController>().ResourcesInArea.RemoveAll(item => item == null);
                                if(Warehouses[0].GetComponent<WarehouseController>().ResourcesInArea.Count >0){
                                    foreach( GameObject resource in Warehouses[0].GetComponent<WarehouseController>().ResourcesInArea){
                                        if(distance > Vector3.Distance(transform.position, resource.transform.position)){
                                            distance = Vector3.Distance(transform.position, resource.transform.position);
                                            if(!resource.GetComponent<ResourceController>().full){
                                                selectedResource = resource;
                                                currentWorkStation = selectedResource;
                                            }
                                        }
                                    }
                                }
                            }
                            MoveUnit(currentWorkStation.transform.position);
                            warehouse.GetComponent<WorkPlaceController>().SetWorker(this);
                        // }
                    }
                    break;
            }
        }
    }

    public void Work()
    {
        // if(currentJob == Job.Generalist){
        //     if(currentWorkStation.GetComponent<WarehouseController>()){
                
        //     }
        // }
        if (!working && State != AIState.OnBreak && energy >0)
        {
            State = AIState.Working;
            working = true;
            anim.SetTrigger("Working");

        }
        if (energy >= 0)
        {
            print(gameObject.name+" Diz : To TORANDO!");
            if (workTimer >= unitWorkStats.workSpeed)
            {
                print(gameObject.name+" Diz : To pegando recurso!");
                
                switch (currentJob)
                {
                    case Job.Farmer:
                        GameObject.Find("GameManager").GetComponent<GameController>().AddFood(unitWorkStats.carryingCapacity);
                        break;
                    case Job.Chef:
                        GameObject.Find("GameManager").GetComponent<GameController>().AddFood(unitWorkStats.carryingCapacity);
                        break;
                    case Job.Guard:
                        //GameController.AddFood(unitWorkStats.carryingCapacity);
                        break;
                    case Job.Generalist:
                        if(currentWorkStation.GetComponent<ResourceController>()){
                            switch (currentWorkStation.GetComponent<ResourceController>().type)
                            {
                                case "TREE":
                                    currentWorkStation.GetComponent<ResourceController>().Collect(unitWorkStats.carryingCapacity);
                                    // print(gameObject.name + " Adicionando " + unitWorkStats.carryingCapacity + " de Wood");
                                    GameObject.Find("GameManager").GetComponent<GameController>().AddWood(unitWorkStats.carryingCapacity);
                                    break;
                                default:
                                    break;
                            }
                        }else if(!currentWorkStation){
                            
                        }
                        //GameController.AddFood(unitWorkStats.carryingCapacity);
                        break;
                }
                workTimer = 0;
                energy -= 10;
            }
        }

    }

    public void SearchForNewResource(){
        // print(gameObject.name+" Diz : Procurando novos Recursos");
        var distance = 10000000f;
        GameObject selectedResource;

        Warehouses[0].GetComponent<WarehouseController>().ResourcesInArea.RemoveAll(item => item == null);

        // print("ResourcesInArea.Count = " + Warehouses[0].GetComponent<WarehouseController>().ResourcesInArea.Count);

        if(Warehouses[0].GetComponent<WarehouseController>().ResourcesInArea.Count >=1){
            foreach( GameObject resource in Warehouses[0].GetComponent<WarehouseController>().ResourcesInArea){
                if(resource){
                    if(distance > Vector3.Distance(transform.position, resource.transform.position)){
                        distance = Vector3.Distance(transform.position, resource.transform.position);
                        if(!resource.GetComponent<ResourceController>().full){
                            // print( gameObject.name+" Diz : Achei um novo recurso disponivel!");
                            selectedResource = resource;
                            currentWorkStation = selectedResource;
                        }else{
                            // print( gameObject.name+" Diz : Não tem espaço pra mim em nenhum recurso!");
                        }
                    }
                }
            }
        }else{
            // print( gameObject.name+" Diz : Não achei nenhum recurso disponivel!");
            var distanceWarehouse = 1000000f;
            foreach( GameObject warehouse in Warehouses){
                if(distanceWarehouse > Vector3.Distance(transform.position, warehouse.transform.position)){
                    distanceWarehouse = Vector3.Distance(transform.position, warehouse.transform.position);
                    currentWorkStation = warehouse;
                    // print( gameObject.name+" Diz : Vou voltar pro Depósito!");
                }
            }
        }
    }
    public void EndShift()
    {
        if (State == AIState.Working)
        {
            switch (currentJob)
            {
                case Job.Farmer:
                    working = false;
                    currentWorkStation.GetComponent<WorkPlaceController>().SetWorker(null);
                    
                    break;
                case Job.Guard:
                    working = false;
                    currentWorkStation.GetComponent<WorkPlaceController>().SetWorker(null);
                    transform.position = currentWorkStation.transform.Find("DoorPoint").transform.position;
                    navAgent.enabled = true;
                    break;
                case Job.Chef:
                    //currentJob = Job.Chef;
                    break;
                case Job.Generalist:
                    working = false;
                    // print( gameObject.name+" Diz : Cabou meu Job");
                    // currentWorkStation.GetComponent<WorkPlaceController>().SetWorker(null);
                    //currentJob = Job.Chef;
                    break;
            }
        }
    }
    public void Sleep()
    {
        if(State == AIState.GoingToBed)
        {
            State = AIState.Sleeping;
        }
        if(State == AIState.Sleeping)
        {
            anim.gameObject.SetActive(false);
            if(energy < 100)
            {
                energy += 0.2f * EnergyRecoverRate;
            }
            else
            {
                Eat();
                energy = 100;
                transform.position = currentWorkStation.transform.Find("DoorPoint").transform.position;
                currentWorkStation.GetComponent<HouseController>().SubPerson();
                anim.gameObject.SetActive(true);
                State = AIState.GoingToWork;
            }
        }
    }
    public void Eat(){  
        if(GameObject.Find("GameManager").GetComponent<GameController>().GetFood() > 0){
            Hungry = false;
            GameObject.Find("GameManager").GetComponent<GameController>().AddFood(-1);
        }else{
            Hungry = true;
        }
    }

    public void CheckAlertStatus(){
        if(Hungry){
            HungryIcon.SetActive(true);
        }else{
            HungryIcon.SetActive(false);
        }
        if(Thirsty){
            ThirstyIcon.SetActive(true);
        }else{
            ThirstyIcon.SetActive(false);
        }
        if(Ill){
            IllIcon.SetActive(true);
        }else{
            IllIcon.SetActive(false);
        }
    }

    public void CheckAnyConditionAffectingEnergyRecovery(){
        if(Hungry || Thirsty || Ill){
            EnergyRecoverRate = 0.5f;
        }else{
            EnergyRecoverRate = 1f;
        }
    }

    private void OnTriggerEnter( Collider other){
        if (other.CompareTag("Tree"))
        {
            if (currentJob == Job.Generalist && State == AIState.GoingToWork && other.gameObject == currentWorkStation){
                other.GetComponent<ResourceController>().AddWorker();
                print( gameObject.name+" Diz : Agr to na arvore!");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Farm"))
        {
            if (currentJob == Job.Farmer && State == AIState.GoingToWork)
                Work();
        }
        if (other.CompareTag("Tree"))
        {
            if (currentJob == Job.Generalist && (State == AIState.GoingToWork || State == AIState.Working)){
                // if(other.GetComponent<ResourceController>().IsWorker(this)){
                //     print("DAME DA NE");
                // }
                Work();
                // if(other.GetComponent<ResourceController>().AddWorker()){
                //     Work();
                // }
            }
        }
        if (other.CompareTag("Tower"))
        {
            if (currentJob == Job.Guard && State == AIState.GoingToWork)
                navAgent.enabled = false;
            //navAgent.destination = currentWorkStation.transform.Find("SpawnGuard").transform.position;
            transform.position = currentWorkStation.transform.Find("SpawnGuard").transform.position;
            Work();
        }
        if (other.CompareTag("House"))
        {
            if(State == AIState.GoingToBed || State == AIState.Sleeping)
                Sleep();
        }
    }
    // private void OnTriggerExit(Collider other){
    //     if (other.CompareTag("Tree"))
    //     {
    //         // print("Saí da Arvore");
    //         if (currentJob == Job.Generalist)
    //             SearchForNewResource();
    //             // Work();
    //     }
    // }
}

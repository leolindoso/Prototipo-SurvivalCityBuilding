using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public ResourceStats resourceStats;
    public int resourceAmount;
    public int numWorkers;
    public int maxWorkers;
    public bool full;
    public string type;
    public List<PlayerUnitController> Workers;
    // Start is called before the first frame update
    void Start()
    {
        resourceAmount = resourceStats.ResourceProvided;
    }

    // Update is called once per frame
    void Update()
    {
        if(numWorkers >= maxWorkers){
            full = true;
        }
        if(resourceAmount<=0){
            // print("IM YELLING TIMBER!!!");
            GetComponent<BoxCollider>().enabled = false;
            foreach (PlayerUnitController playerUnit in Workers){
                playerUnit.SearchForNewResource();
            }
            Destroy(gameObject);
        }
    }
    public bool AddWorker(){
        if(!full){
            numWorkers++;
            return true;
        }
        return false;
    }

    public bool IsWorker(PlayerUnitController playerUnit){
        if(Workers.Contains(playerUnit)){
            return true;
        }
        return false;
    }
    public int Collect(int amount){
        resourceAmount -= amount;
        return amount;
    }
    
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("PlayerUnit") && !full){
            Workers.Add(other.gameObject.GetComponent<PlayerUnitController>());
        }
    }
}

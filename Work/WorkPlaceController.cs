using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkPlaceController : MonoBehaviour
{
    public PlayerUnitController currentWorker;
    public bool busy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWorker != null)
        {
            busy = true;
        }
        else
        {
            busy = false;
        }
    }
    public void SetWorker(PlayerUnitController worker)
    {
        currentWorker = worker;
    }
}

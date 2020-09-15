using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpeditorsTeamController : MonoBehaviour
{
    public GameObject[] explorers;
    public Transform[] spawnPoint;

    private void Awake()
    {
        explorers = GameObject.FindGameObjectsWithTag("PlayerUnit");
        StartCoroutine(ResetExplorersPosition());
    }
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator ResetExplorersPosition()
    {
        yield return new WaitForSeconds(0.03f);

        int i = 0;
        foreach (GameObject explorer in explorers)
        {
            Debug.Log(explorer.name + " - " + i);
            explorer.transform.position = spawnPoint[i].position;
            i++;
        }
    }
}

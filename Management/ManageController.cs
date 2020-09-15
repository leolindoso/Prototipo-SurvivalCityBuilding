using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageController : MonoBehaviour
{
    public GameObject[] population;
    public PlayerUnitController selectedPerson;
    public Text selectedJob;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePopulation();
    }
    public void UpdatePopulation()
    {
        population = GameObject.FindGameObjectsWithTag("PlayerUnit");
    }
    public void SetSelectedPerson(GameObject button)
    {
        selectedPerson = GameObject.Find(button.transform.Find("Name").GetComponent<Text>().text).GetComponent<PlayerUnitController>();

    }

    public void AssignJob()
    {
        selectedPerson.SetJob(selectedJob.text);
    }
}

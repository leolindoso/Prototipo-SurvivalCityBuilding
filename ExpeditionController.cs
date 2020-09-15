using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ExpeditionController : MonoBehaviour
{
    public string destination;
    public GameObject[] avaliableExplorers;
    public List<GameObject> explorers;
    // Start is called before the first frame update
    void Start()
    {
        explorers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        avaliableExplorers = GameObject.FindGameObjectsWithTag("PlayerUnit");
    }

    public void SetDestination(string dest)
    {
        destination = dest;
    }

    public void ExploreDestination()
    {
        foreach(GameObject explorer in explorers)
        {
            DontDestroyOnLoad(explorer);
        }
        
        SceneManager.LoadScene(destination);
    }

    public void AddExplorer(GameObject button)
    {
        explorers.Add(GameObject.Find(button.transform.Find("Name").GetComponent<Text>().text));
    }
}

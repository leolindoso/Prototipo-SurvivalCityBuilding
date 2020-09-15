using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public HousingStats housingStats;
    public int peopleInside;
    public bool full;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GameManager").GetComponent<GameController>().AddHousing(housingStats.housingProvided);
    }

    // Update is called once per frame
    void Update()
    {
        if(peopleInside >= housingStats.housingProvided)
        {
            full = true;
        }
        else
        {
            full = false;
        }
    }
    public bool AddPerson()
    {
        if(peopleInside < housingStats.housingProvided){
            peopleInside++;
            return true;
        }
        return false;
    }
    public void SubPerson()
    {
        peopleInside--;
    }
}

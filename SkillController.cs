using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public List<SkillController> dependsOn;
    public bool unlocked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Button>().interactable = CheckDependencies();
    }

    private bool CheckDependencies()
    {
        if (dependsOn.Count == 0)
        {
            return true;
        }
        else
        {
            bool aux = true;
            foreach(SkillController skill in dependsOn)
            {
                if (!skill.unlocked)
                {
                    aux = false;
                    
                }
                
            }
            return aux;
        }
    }
    public void UnlockThis()
    {
        unlocked = true;
    }
}

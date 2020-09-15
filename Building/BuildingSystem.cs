using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public enum GameState { Building, Playing };
       
    public GameState state = GameState.Building;
    public Transform objToMove;
    public GameObject objToPlace;
    public LayerMask mask;
    GameObject builtObject;
    float lastPosX, lastPosY, lastPosZ;
    Vector3 mousePosition;
    bool CanBuild = false;

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if(objToPlace && state != GameState.Playing){
            if( GameObject.Find("GameManager").GetComponent<GameController>().GetWood() >= objToPlace.GetComponent<BuildingConstructionController>().Wood && GameObject.Find("GameManager").GetComponent<GameController>().GetMetal() >= objToPlace.GetComponent<BuildingConstructionController>().Metal ){
                CanBuild = true;
            }else{
                CanBuild = false;
            }
            if(!CanBuild && objToMove){
                for(int i = 0; i< objToMove.transform.childCount;i++){
                    objToMove.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = new Color(1,0,0,0.5f);

                }
            }
        }


        if (Input.GetKeyDown(KeyCode.F2))
        {
            state = GameState.Building;
            
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            state = GameState.Playing;
            objToPlace = null;
            objToMove.gameObject.SetActive(false);
        }


        if (state == GameState.Building)
        {
            objToMove.gameObject.SetActive(true);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                float posY = hit.point.y;
                float posZ = hit.point.z;
                float posX = hit.point.x;

                objToMove.localPosition = new Vector3(posX, posY, posZ);
                if (posX != lastPosX || posY != lastPosY || posZ != lastPosZ)
                {
                    lastPosX = posX;
                    lastPosY = posY;
                    lastPosZ = posZ;
                
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (state == GameState.Building && CanBuild)
                    {
                        GameObject.Find("GameManager").GetComponent<GameController>().AddWood(-objToPlace.GetComponent<BuildingConstructionController>().Wood);
                        GameObject.Find("GameManager").GetComponent<GameController>().AddMetal(-objToPlace.GetComponent<BuildingConstructionController>().Metal);
                        GameObject building = Instantiate(objToPlace, objToMove.position, objToMove.rotation);
                        builtObject = building;
                        //state = CursorState.Playing;
                    }
                
                }
                if (Input.GetMouseButtonDown(1))
                {
                    state = GameState.Playing;
                    objToPlace = null;
                    objToMove.gameObject.SetActive(false);

                }
                if (Input.GetMouseButtonUp(0))
                {
                    builtObject = null;
                    state = GameState.Building;
                }
                if (Input.GetKeyDown(KeyCode.Comma))
                {
                    Debug.Log("<");
                    objToMove.transform.Rotate(objToMove.transform.rotation.x, objToMove.transform.rotation.y - 45, objToMove.transform.rotation.z);
                    //builtObject.transform.Rotate(objToMove.transform.rotation.x, objToMove.transform.rotation.y +45, objToMove.transform.rotation.z); 
                }
                if (Input.GetKeyDown(KeyCode.Period))
                {
                    Debug.Log(">");
                    objToMove.transform.Rotate(objToMove.transform.rotation.x, objToMove.transform.rotation.y + 45, objToMove.transform.rotation.z);
                }
            }
        }
    }
    public void ChangeBuilding(GameObject newBuilding)
    {
        objToPlace = newBuilding;
    }
    public void ChangePlaceholder(GameObject newPlaceholder)
    {
        objToMove = newPlaceholder.transform;
    }
    public void StartBuilding()
    {
        state = GameState.Building;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseController : MonoBehaviour
{
    public GameObject canvas;
    public enum GameState { Managing, Exploring, ChangingWorkArea};
    public GameState state;
    public CameraTeste1 cameraTeste1;
    public CameraController cameraController;
    public GameObject managingHUD;
    public GameController game;
    public LayerMask maskGround;
    public LayerMask maskResources;
    public PlayerPawnController player;
    public float radius;
    public GameObject workAreaPoint;
    public LineRenderer workAreaLine;
    public Vector3 lastWorkAreaPos;
    public List<GameObject> ResourcesInArea;

    // Start is called before the first frame update
    void Start()
    {
        
        workAreaLine.positionCount =  25 + 1;
        workAreaLine.useWorldSpace = false;
        CheckForResourcesInArea();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(state != GameState.Exploring){
            cameraTeste1.enabled = false;
            cameraController.enabled = true;
            workAreaLine.gameObject.GetComponent<MeshRenderer>().enabled = true;
            workAreaLine.enabled = true;
            CreateLinePoints();
        }else{
            workAreaLine.gameObject.GetComponent<MeshRenderer>().enabled = false;
            workAreaLine.enabled = false;
        }
        if(state == GameState.ChangingWorkArea){
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, maskGround))
            {
                float posY = hit.point.y;
                float posZ = hit.point.z;
                float posX = hit.point.x;

                workAreaLine.transform.position = new Vector3(posX, posY, posZ);
                // if (posX != lastWorkAreaPos.x || posY != lastWorkAreaPos.y || posZ != lastWorkAreaPos.z)
                // {
                //     lastWorkAreaPos.x = posX;
                //     lastWorkAreaPos.y = posY;
                //     lastWorkAreaPos.z = posZ;
                
                // }
                if (Input.GetMouseButtonDown(0))
                {
                    if (state == GameState.ChangingWorkArea)
                    {
                        workAreaPoint.transform.position = new Vector3(posX,posY,posZ);
                        CheckForResourcesInArea();
                        // lastWorkAreaPos = new Vector3(posX,posY,posZ);
                        // state = GameState.Managing;
                        // ActivateCanvas();
                        ActivateManaging();
                        // GameObject.Find("GameManager").GetComponent<GameController>().AddWood(-objToPlace.GetComponent<BuildingConstructionController>().Wood);
                        // GameObject.Find("GameManager").GetComponent<GameController>().AddMetal(-objToPlace.GetComponent<BuildingConstructionController>().Metal);
                        // GameObject building = Instantiate(objToPlace, objToMove.position, objToMove.rotation);
                        // builtObject = building;
                        //state = CursorState.Playing;
                    }
                
                }
                if (Input.GetMouseButtonDown(1))
                {
                    state = GameState.Managing;
                    workAreaPoint.transform.position = lastWorkAreaPos;
                    ActivateCanvas();
                    // objToPlace = null;
                    // objToMove.gameObject.SetActive(false);

                }
                // if (Input.GetMouseButtonUp(0))
                // {
                //     builtObject = null;
                //     state = GameState.Building;
                // }
                // if (Input.GetKeyDown(KeyCode.Comma))
                // {
                //     Debug.Log("<");
                //     objToMove.transform.Rotate(objToMove.transform.rotation.x, objToMove.transform.rotation.y - 45, objToMove.transform.rotation.z);
                //     //builtObject.transform.Rotate(objToMove.transform.rotation.x, objToMove.transform.rotation.y +45, objToMove.transform.rotation.z); 
                // }
                // if (Input.GetKeyDown(KeyCode.Period))
                // {
                //     Debug.Log(">");
                //     objToMove.transform.Rotate(objToMove.transform.rotation.x, objToMove.transform.rotation.y + 45, objToMove.transform.rotation.z);
                // }
            }
        }
    }

    public void CheckForResourcesInArea(){
        print("Checando Area...");
        RaycastHit hit;
        Collider[] resources = Physics.OverlapSphere(workAreaPoint.transform.position,radius);
        foreach (Collider col in resources)
        {
            if( maskResources == (maskResources | (1 << col.gameObject.layer))){
                if(!ResourcesInArea.Contains(col.gameObject)){
                    ResourcesInArea.Add(col.gameObject);
                    print(col.transform.name);
                }
            }
        }
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(workAreaPoint.transform.position + Vector3.zero ,radius);
    }
    void CreateLinePoints ()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (25 + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;

            workAreaLine.SetPosition (i,new Vector3(x,0,y) );

            angle += (360f / 25);
        }
    }
    public void ActivateManaging()
    {
        state = GameState.Managing;
        lastWorkAreaPos = workAreaPoint.transform.position;
        managingHUD.SetActive(true);
        player.enabled = false;
    }
    public void ChangingWorkArea(){
        state = GameState.ChangingWorkArea;
        
    }
    public void DesactivateManaging()
    {
        state = GameState.Exploring;
        player.enabled = true;
        managingHUD.SetActive(false);
        cameraTeste1.enabled = true;
        cameraController.enabled = false;
    }
    public void ActivateCanvas()
    {
        canvas.SetActive(true);

    }
    public void DesactivateCanvas()
    {
        canvas.SetActive(false);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    RaycastHit hit;
    public List<UnitController> selectedUnits = new List<UnitController>();
    bool isDragging = false;
    Vector3 mousePosition;

    private void OnGUI()
    {
        if (isDragging)
        {
            //Rect rect = ScreenHelper.GetScreenRect(mousePosition,Input.mousePosition);
            //ScreenHelper.DrawScreenRect(rect, new Color(1,1,1,0.3f));
            //ScreenHelper.DrawScreenRectBorder(rect, 2f,new Color(1, 1, 1, 0.6f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //DETECTA O CLICK
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            //CRIA O RAIO DA CAMERA PRO MUNDO
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            //ATIRA O RAIO E PEGA OS DADOS DO HIT
            if (Physics.Raycast(camRay, out hit))
            {
                //USA OS DADOS DO HIT
                if (hit.transform.CompareTag("PlayerUnit"))
                {
                    SelectUnit(hit.transform.gameObject.GetComponent<UnitController>(), Input.GetKey(KeyCode.LeftControl));
                }else
                {
                    isDragging = true;
                }
            }
        }
        //AO SOLTAR O CLICK
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
            DeselectUnits();
            }
            //VERIFICA SE OS OBJETOS COM PLAYERUNITCONTROLLER ESTÃO DENTRO DA AREA DE SELECAO
            foreach(PlayerUnitController unit in FindObjectsOfType<PlayerUnitController>())
            {
                if (IsWithinSelectionBounds(unit.transform))
                {
                    //SelectUnit(unit, true);
                }
            }
            isDragging = false;
        }
        //DEFINE DESTINO DAS UNIDADES SELECIONADAS
        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out hit))
            {



                //INTERAÇÕES
                Debug.Log(hit.transform);
                if (hit.transform.CompareTag("Ground"))
                {
                    foreach(UnitController unit in selectedUnits)
                    {
                        unit.MoveUnit(hit.point);
                    }
                }else if (hit.transform.CompareTag("Enemy"))
                {
                    foreach (UnitController unit in selectedUnits)
                    {
                        unit.FollowUnit(hit.transform.gameObject);
                    }
                }else if (hit.transform.CompareTag("Farm"))
                {
                    foreach (UnitController unit in selectedUnits)
                    {
                        unit.SetJob(hit.point,"Farmer");
                    }
                }
            }
        }
    }

    //SELECIONA UNIDADE, SE MULTISELECT, SELECIONA VARIAS
    private void SelectUnit(UnitController unit, bool multiSelect = false)
    {
        if (!multiSelect)
        {
            DeselectUnits();
        }
        
        selectedUnits.Add(unit);
        unit.SetSelected(true);
    }

    //DESELECIONA AS UNIDADES E LIMPA A LISTA
    private void DeselectUnits()
    {
        // foreach(UnitController unit in selectedUnits)
        // {
        //     unit.SetSelected(false);
        // }
        // selectedUnits.Clear();
    }

    //CHECKA SE O OBJETO, CUJO TRANSFORM FOI PASSADO COMO PARAMETRO, ESTÁ DENTRO DA AREA DE SELECAO
    private bool IsWithinSelectionBounds(Transform transform)
    {
        if (!isDragging)
        {
            return false;
        }
        Camera camera = Camera.main;
        Bounds viewportBounds = ScreenHelper.GetViewportBounds(camera,mousePosition,Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(transform.position));
    }
}

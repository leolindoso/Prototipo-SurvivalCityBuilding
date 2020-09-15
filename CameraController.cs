using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 20f;
    public float XRotation = 0f;
    public float YRotation = 0f;
    public float lastYRotation;
    public Transform pivot;
    // Start is called before the first frame update
    void Start()
    {
        ;
    }

    // Update is called once per frame
    void Update()
    {
        
        float MouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;

        Vector3 pos = transform.localPosition;
        if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            
            Vector3 aux = new Vector3(0, 0, panSpeed);
            transform.Translate(aux, pivot);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <=  panBorderThickness)
        {
            Vector3 aux = new Vector3(0, 0, -panSpeed);
            transform.Translate(aux, pivot);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            Vector3 aux = new Vector3(panSpeed, 0, 0);
            transform.Translate(aux, pivot);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <=panBorderThickness)
        {
            Vector3 aux = new Vector3(-panSpeed, 0, 0);
            transform.Translate(aux, pivot);
        }
        
        XRotation -= MouseY;
        YRotation += MouseX;
        XRotation = Mathf.Clamp(XRotation, -60f, 0f);
        //YRotation = Mathf.Clamp(YRotation, -90f, 90f);
        if (Input.GetMouseButtonDown(2))
        {
            if (Mathf.Abs(lastYRotation) > 0)
            {

                YRotation = lastYRotation;
            }
        }

        if (Input.GetMouseButton(2))
        {
            
            transform.localRotation = Quaternion.Euler(50f, YRotation, 0f);
            //transform.Rotate(Vector3.up * MouseX);
        }
        if (Input.GetMouseButtonUp(2))
        {
            lastYRotation += YRotation - lastYRotation;
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f* Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, 20f, 50f);
        Vector3 aux1 = new Vector3(0, 0, scroll*2);
        if((transform.localPosition.y > 10 && scroll > 0) || (transform.localPosition.y < 20 && scroll <0))
        {
            
            transform.Translate(aux1, Space.Self);
        }
        
        //transform.localPosition = new Vector3(transform.localPosition.x,pos.y, transform.localPosition.z);
    }
    void OnEnable(){
        
    }
}

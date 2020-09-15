using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTeste1 : MonoBehaviour
{
    public float XRotation = 0f;
    public float YRotation = 0f;
    public Transform Target;
    public Transform cameraMain;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;

        XRotation -= MouseY;
        YRotation += MouseX;
        XRotation = Mathf.Clamp(XRotation,-60f,0f);
        //YRotation = Mathf.Clamp(YRotation, -90f, 90f);
        transform.position = Target.position;
        if (Input.GetMouseButton(2))
        {
            //transform.localRotation = Quaternion.Euler(50f, YRotation, 0f);
            transform.Rotate(Vector3.up * MouseX);
        }
    }
    private void OnEnable()
    {
        cameraMain.localPosition = new Vector3(0f, 10f, -8f);
        cameraMain.localRotation = Quaternion.Euler(50f, 0f, 0f);
    }
}

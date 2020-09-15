using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHallController : MonoBehaviour
{
    public GameObject canvas;
    public enum GameState { Managing, Exploring };
    public GameState state;
    public CameraTeste1 cameraTeste1;
    public CameraController cameraController;
    public GameObject managingHUD;
    public GameController game;
    public PlayerPawnController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.Managing)
        {
            
            cameraTeste1.enabled = false;
            cameraController.enabled = true;
        }else if(state == GameState.Exploring)
        {
            
        }
    }
    public void ActivateManaging()
    {
        state = GameState.Managing;
        managingHUD.SetActive(true);
        player.enabled = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public float temperature;
    public enum Season { Spring, Summer, Fall, Winter}
    public Season actualSeason;
    private float actualMax;
    private float actualMin;
    float targetTemperature;
    public float timeStartedLerp;
    float lerpTime;
    [Header("Spring")]
    public float springMax;
    public float springMin;
    [Header("Summer")]
    public float summerMax;
    public float summerMin;
    [Header("Fall")]
    public float fallMax;
    public float fallMin;
    [Header("Winter")]
    public float winterMax;
    public float winterMin;


    public void StartLerping()
    {
        timeStartedLerp = Time.time;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(LightingController.timeDay.ToString("0.00"));
        if(LightingController.timeDay.ToString("0.00") == "0,00")
        {
            
            GetComponent<GameController>().AddDay();
            temperature = actualMin;
        }


        if ((LightingController.timeDay >= 11.9f && LightingController.timeDay <= 12.1f) || ((LightingController.timeDay >= 23.8f && LightingController.timeDay <= 24.1f)))
        {
            StartLerping();
        }
        if (LightingController.timeDay/24 > 0.5f)
        {
            
            targetTemperature = actualMin;
            temperature = LerpTemperature(false);
        }
        else
        {
            
            targetTemperature = actualMax;
            temperature = LerpTemperature(true);
        }
        
    }

    public void SetActualMax(float max)
    {
        actualMax = max;
    }
    public void SetActualMin(float min)
    {
        actualMin = min;
    }

    public float LerpTemperature(bool warming = true)
    {
        float timeSinceStarted = Time.time - timeStartedLerp;
        float percentage = timeSinceStarted / (GetComponent<LightingController>().dayDurationSeconds/2);
        float result = 0;
        if (warming)
        {
            result = Mathf.Lerp(actualMin, actualMax, percentage);
        }
        else
        {
            result = Mathf.Lerp(actualMax, actualMin, percentage);
        }
        
        return result;
    }
}

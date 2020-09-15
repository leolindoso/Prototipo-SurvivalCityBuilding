using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingController : MonoBehaviour
{
    public Light directionalLight;
    public LightingStats preset;
    [Range(0, 24)]
    public static float timeDay;
    public float dayDurationSeconds;
    public float timeStartedLerping;
    public float StarterHour;

    public void StartLerping()
    {
        if(timeStartedLerping == 0){
            timeStartedLerping = Time.time;
        }
    }
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);
        if(directionalLight != null)
        {
            directionalLight.color = preset.ambientColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));

        }
    }

    private void OnValidate()
    {
        if(directionalLight != null)
        {
            return;
        }
        if(RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartLerping();
    }

    // Update is called once per frame
    void Update()
    {
        if(preset == null) { 
            return;
        }
        //timeDay += 1f * Time.fixedDeltaTime;
        //timeDay %= 24;
        if(timeDay > 12f && timeDay < 12.5f){
            print(Time.time - timeStartedLerping);
        }
        
        if(timeDay >= 23.99f)
        {
            timeDay = 0f;
            StartLerping();
        }
        timeDay = LerpTimeDay();
        UpdateLighting(timeDay / 24f);


    }
    public float LerpTimeDay()
    {   
        float timeSinceStarted = Time.time - timeStartedLerping;
        if(StarterHour != 0){
            timeSinceStarted = StarterHour;
            StarterHour = 0;
        }
        float percentage = timeSinceStarted / dayDurationSeconds;
        float result = Mathf.Lerp(0, 24, percentage);
        return result;
    }
}

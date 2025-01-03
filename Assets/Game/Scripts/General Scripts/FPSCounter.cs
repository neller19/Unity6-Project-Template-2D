using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    //can set a FPS value, under which this thing will send off an alert. Allows me to respond to events while testing performance, and stop
    //doing stuff (eg spawning enemies) when FPS gets too low
    public System.Action FPSAlert;

    /* Assign this script to any object in the Scene to display frames per second */

    public float updateInterval = 0.5f; //How often should the number update

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;

    GUIStyle textStyle = new GUIStyle();

    int FPSAlertLimit = int.MaxValue;

    // Use this for initialization
    void Start()
    {
        timeleft = updateInterval;
        textStyle.fontSize = 30;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;

            if (fps < FPSAlertLimit)
            {
                FPSAlert?.Invoke();
            }
        }
               
    }

    public void SetFPSAlert(int limit)
    {
        FPSAlertLimit = limit;
    }
    void OnGUI()
    {
        //Display the fps and round to 2 decimals
        GUI.Label(new Rect(5, 5, 100, 25), fps.ToString("F2") + "FPS", textStyle);
    }
}
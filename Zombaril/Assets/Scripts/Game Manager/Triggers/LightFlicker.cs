using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light lightToControl;  // variable for light
    public float minTime = 3f;    // set default values for minTime and maxTime
    public float maxTime = 5f;

    private float timer;         // set the access modifier to private to prevent it from being modified from outside the class

    private void Start()
    {
        timer = Random.Range(minTime, maxTime);
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            // toggle the enabled state of the light
            lightToControl.enabled = !lightToControl.enabled;

            // reset the timer with a random value between minTime and maxTime
            timer = Random.Range(minTime, maxTime);
        }
    }
}

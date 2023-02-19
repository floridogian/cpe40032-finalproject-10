using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    public float swayIntensity = 1.0f; // Controls the intensity of the sway.
    public float swaySmoothness = 5.0f; // Controls how smoothly the weapon sways.
    private Quaternion originalRotation; // The original rotation of the weapon, before any sway.

    private void Start()
    {
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        // Get the mouse input for the x and y axes.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate the target rotation based on the mouse input.
        Quaternion xRotation = Quaternion.AngleAxis(-swayIntensity * mouseX, Vector3.up);
        Quaternion yRotation = Quaternion.AngleAxis(swayIntensity * mouseY, Vector3.right);
        Quaternion targetRotation = originalRotation * xRotation * yRotation;

        // Smoothly rotate towards the target rotation.
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swaySmoothness);
    }
}
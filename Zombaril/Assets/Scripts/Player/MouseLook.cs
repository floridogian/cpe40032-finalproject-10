using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    // Public variables
    public float mouseSensitivity = 100f;  // Sensitivity of mouse look
    public Transform player;               // Reference to player character transform
    public Slider sensitivitySlider;       // A reference to the sensitivity slider

    // Private Variables
    private float xRotation = 0f;          // Rotation around X axis

    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("currentSensitivity", mouseSensitivity);
        sensitivitySlider.value = mouseSensitivity / 10;
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        PlayerPrefs.SetFloat("currentSensitivity", mouseSensitivity);
        // Get the mouse input values
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the camera around the X axis (up and down) based on the vertical mouse input
        xRotation -= mouseY;
        // Clamp the X rotation to prevent the camera from flipping over
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the X rotation to the camera transform
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Apply the horizontal mouse input to the player character transform
        player.Rotate(Vector3.up * mouseX);
    }

    public void AdjustSpeed(float newSensitivitySpeed)
    {
        mouseSensitivity = newSensitivitySpeed * 10;
    }
}
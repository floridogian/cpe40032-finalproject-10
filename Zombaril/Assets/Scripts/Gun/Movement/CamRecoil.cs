using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRecoil : MonoBehaviour
{
    // Recoil Settings
    public float rotationSpeed = 6;
    public float returnSpeed = 25;

    // Recoil rotation amount for hipfire and aiming
    public Vector3 hipfireRecoilRotation = new Vector3(2f, 2f, 2f);
    public Vector3 aimingRecoilRotation = new Vector3(0.5f, 0.5f, 1.5f);

    // State of the player's aim
    private bool isAiming;

    // The current rotation of the camera
    private Vector3 currentRotation;

    // Initialize currentRotation when the game starts
    private void Start()
    {
        currentRotation = Vector3.zero;
    }

    // Update the rotation of the camera when the player fires
    private void FixedUpdate()
    {
        // Gradually decrease the currentRotation towards zero using the returnSpeed
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);

        // Gradually rotate the camera using the rotationSpeed and the current rotation
        Vector3 rotation = Vector3.Slerp(transform.localRotation.eulerAngles, currentRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(rotation);
    }

    // Apply recoil to the camera when the player fires
    public void Fire()
    {
        // Get the recoil rotation based on whether the player is aiming or hipfiring
        Vector3 recoilRotation = isAiming ? aimingRecoilRotation : hipfireRecoilRotation;

        // Add random rotation to the current rotation, creating a random recoil pattern
        currentRotation += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
    }

    // Change the recoil pattern based on the player's aim
    private void Update()
    {
        // Fire when the player presses the fire button
        if (Input.GetMouseButton(0))
        {
            Fire();
        }

        // Toggle aiming state when the player presses the aim button
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }
    }
}

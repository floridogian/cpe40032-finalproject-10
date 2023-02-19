using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Initialize cam variable
    public Transform cam;

    // Make the healthBar of the enemy face at the camera
    void LateUpdate()
    {
        transform.LookAt(cam);
    }
}

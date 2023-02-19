using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    // Variables to store the current and target rotations of the weapon game object
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    // Variables to control the magnitude of the recoil effect
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    // Variables to control the snappiness and return speed of the recoil effect
    public float snappiness;
    public float returnSpeed;

    private void Update()
    {
        // Gradually return the target rotation back to zero over time
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        // Update the current rotation to the target rotation with a smoothing effect
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        // Apply the rotation to the weapon game object
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Recoil()
    {
        // Add a random rotation along the X and Z axes to the target rotation
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
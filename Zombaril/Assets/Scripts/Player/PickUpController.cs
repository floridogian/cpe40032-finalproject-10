using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpController : MonoBehaviour
{
    // Variables for the Gun script, Rigidbody component, BoxCollider component, player transform, gun container transform, and first-person camera transform
    public Gun gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;

    // Variables for pick-up range, force applied when dropped, and whether the gun is equipped or not
    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;
    public bool equipped;
    public static bool slotFull;

    // Variables for ammo and medkit pickup
    public int ammoToAdd = 35;
    public int healthToAdd = 25;

    // Pickup text
    public TextMeshProUGUI pickedUp;
    public TextMeshProUGUI pickUpText;

    PlayerController playerController;

    private void Start()
    {
        // Initial setup
        playerController = FindObjectOfType<PlayerController>();

        // If the gun is not equipped:
        if (!equipped)
        {
            // Disable the gun script
            gunScript.enabled = false;
            // Make the rigidbody not kinematic
            rb.isKinematic = false;
            // Make the BoxCollider not a trigger
            coll.isTrigger = false;
        }
        // If the gun is equipped:
        if (equipped)
        {
            // Enable the gun script
            gunScript.enabled = true;
            // Make the rigidbody kinematic
            rb.isKinematic = true;
            // Make the BoxCollider a trigger
            coll.isTrigger = true;
            // Set the slotFull variable to true
            slotFull = true;
        }
    }

    private void Update()
    {
        // Check if the player is in range and "E" is pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            // If all conditions are met, pick up the gun
            PickUp();
        }

        // Check if the gun is equipped and "Q" is pressed
        if (equipped && Input.GetKeyDown(KeyCode.Q))
        {
            // If the conditions are met, drop the gun
            Drop();
        }

        SetTextForInteractableObjects();
    }

    private void PickUp()
    {
        // Play pick and drop sound
        FindObjectOfType<AudioManager>().PlaySound("PickAndDrop");

        // Set equipped to true and slotFull to true
        equipped = true;
        slotFull = true;

        // Make the gun a child of the gun container and set its position, rotation, and scale to default
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        // Make the rigidbody kinematic and the BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        // Enable the gun script
        gunScript.enabled = true;
    }

    public void Drop()
    {
        // Play pick and drop sound
        FindObjectOfType<AudioManager>().PlaySound("PickAndDrop");

        // Set equipped to false and slotFull to false
        equipped = false;
        slotFull = false;

        // Set the parent of the gun to null and set the scale to 0.7
        transform.localScale = Vector3.one * 0.7f;
        transform.SetParent(null);

        // Make the rigidbody not kinematic and the BoxCollider not a trigger
        rb.isKinematic = false;
        coll.isTrigger = false;

        // Gun carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        // Add Force
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        // Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        // Disable script
        gunScript.enabled = false;
    }

    private void SetTextForInteractableObjects()
    {
        // Perform a raycast to detect if the player is looking at an object
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, pickUpRange, LayerMask.GetMask("Pickup")))
        {
            GameObject pickupObject = hit.collider.gameObject;

            // Check if the object's tag is "Ammo" or "Medkit"
            if (pickupObject.CompareTag("Ammo"))
            {
                pickUpText.SetText("Press E to pick up Ammo");
                if (Input.GetKeyDown(KeyCode.E) && equipped)
                {
                    gunScript.ammoInReserve += ammoToAdd;
                    pickedUp.SetText("Picked up " + ammoToAdd + " ammo!");
                    StartCoroutine(PickingUp());
                    Destroy(pickupObject);
                }
                else if (Input.GetKeyDown(KeyCode.E) && !equipped)
                {
                    pickedUp.SetText("You don't have a gun!");
                    StartCoroutine(PickingUp());
                }
            }
            else if (pickupObject.CompareTag("Medkit"))
            {
                pickUpText.SetText("Press E to pick up Medkit");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (playerController.currentHealth < playerController.maxHealth)
                    {
                        player.GetComponent<PlayerController>().GainHealth(healthToAdd);
                        pickUpText.SetText("Medkit");
                        pickedUp.SetText("Gained " + healthToAdd + " health!");
                        StartCoroutine(PickingUp());
                        Destroy(pickupObject);
                    }
                    else
                    {
                        pickedUp.SetText("Your current health is still 100.");
                        StartCoroutine(PickingUp());
                    }
                }
                
            }
            else if (pickupObject.CompareTag("Gun") && !equipped && !slotFull)
            {
                pickUpText.SetText("Press E to pick up Gun");
            }
            else
            {
                // If the object doesn't have a recognized tag, clear the pick up text
                pickUpText.SetText("");
            }
        }
        else
        {
            // If the raycast doesn't hit anything, clear the pick up text
            pickUpText.SetText("");
        }
    }


    IEnumerator PickingUp()
    {
        pickedUp.enabled = true;

        // Wait for 2 seconds
        yield return new WaitForSeconds(2.0f);

        pickedUp.enabled = false;
    }
}

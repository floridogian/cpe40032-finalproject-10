using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    // Gun Settings
    public float fireRate;            // The time (in seconds) between each shot
    public int magazineSize;          // The maximum number of bullets in a magazine
    public int reservedAmmoCapacity;  // The maximum amount of ammo that can be carried
    public int damage;                // The amount of damage dealt by each bullet
    public float hipfireSpread;       // The spread of the bullet when firing from the hip
    public float aimSpread;           // The spread of the bullet when aiming down sights
    public int reloadTime;            // The time (in seconds) it takes to reload the gun

    // Variables that can change throughout code
    bool canShoot;             // Whether the gun can currently be fired
    bool isReloading;          // Whether the gun is currently being reloaded
    int currentAmmo;           // The number of bullets currently in the magazine
    public int ammoInReserve;  // The number of bullets in reserve

    // Muzzle flash and Bullet shot
    public ParticleSystem muzzleFlash;    // The particle system that creates the muzzle flash when firing the gun
    public GameObject bulletHoleGraphic;  // The graphic that appears when a bullet hits a surface
    public RaycastHit hit;                // Information about the object that the bullet hit

    // Recoil
    private GunRecoil gunRecoil;  // A class that handles the recoil of the gun

    // Aiming
    public Vector3 hipfireLocalPosition;  // The local position of the gun when hipfiring
    public Vector3 adsLocalPosition;      // The local position of the gun when aiming down sights
    public float aimSpeed;                // The speed at which the gun moves when aiming down sights

    // Text
    public TextMeshProUGUI magazineText;  // The UI text that displays the number of bullets in the magazine
    public GameObject reloadText;         // The UI text that appears when the gun is being reloaded

    // Other
    public GameObject crosshair;  // The UI crosshair
    public Animator animator;     // The animator component for the gun
    private PlayerController playerController;

    private void Start()
    {
        // Get the player controller script
        playerController = FindObjectOfType<PlayerController>();

        // Get the gunRecoil script to be applied when shooting
        gunRecoil = FindObjectOfType<GunRecoil>();

        // Set current ammo as magazineSize
        currentAmmo = magazineSize;

        // Set ammoInReserve as reservedAmmoCapacity
        ammoInReserve = reservedAmmoCapacity;

        // Set isReloading to false and canShoot to true
        isReloading = false;
        canShoot = true;
    }

    private void Update()
    {
        UpdateAmmo();
        Aim();
        ShootGun();
        ReloadGun();
    }

    // Update gun mechanics
    private void UpdateAmmo()
    {
        // Display the current ammo
        magazineText.SetText("Ammo: " + currentAmmo + " / " + ammoInReserve);
    }

    // Transform the position of the gun when aiming
    private void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            // Move gun to the aim down sights position and disable the crosshair
            transform.localPosition = Vector3.Lerp(transform.localPosition, adsLocalPosition, Time.deltaTime * aimSpeed);
            crosshair.SetActive(false);
            playerController.speed = 4.0f;
        }
        else
        {
            // Move gun to the hipfire position and enable the crosshair
            transform.localPosition = Vector3.Lerp(transform.localPosition, hipfireLocalPosition, Time.deltaTime * aimSpeed);
            crosshair.SetActive(true);
            playerController.speed = 6.0f;
        }
    }

    // Shoot gun
    private void ShootGun()
    {
        // Check if player is pressing the left mouse button and the gun can shoot
        if (Input.GetMouseButton(0) && canShoot && currentAmmo > 0)
        {
            // Disable shooting until the coroutine is complete
            canShoot = false;

            // Decrease current ammo count
            currentAmmo--;

            // Start the Shoot coroutine
            StartCoroutine(Shoot());
        }
    }

    // Reload gun
    void ReloadGun()
    {
        // Check if player is pressing R and the gun is not already reloading
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magazineSize && ammoInReserve > 0 && !isReloading)
        {
            // Start the Reload coroutine
            StartCoroutine(Reload());
        }

        // Check if current ammo is 0 and the gun is not already reloading and there is ammo in reserve
        else if (currentAmmo == 0 && !isReloading && ammoInReserve != 0)
        {
            // Start the Reload coroutine
            StartCoroutine(Reload());
        }
    }

    // Coroutine for shooting
    IEnumerator Shoot()
    {
        // Wait for the fire rate of the gun
        yield return new WaitForSeconds(fireRate);

        // Play the muzzle flash particle effect and shoot sound
        muzzleFlash.Play();
        FindObjectOfType<AudioManager>().PlaySound("Shoot");

        // Apply recoil to the gun and move it back slightly
        gunRecoil.Recoil();
        transform.localPosition -= Vector3.forward * 0.1f;

        // Get the spread of the gun based on whether the player is aiming or not
        float spread = Input.GetKey(KeyCode.V) ? aimSpread : hipfireSpread;

        // Calculate the direction the bullet will travel using random values
        Vector3 spreadDirection = new Vector3(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread),
            Random.Range(-spread, spread)
        );

        Vector3 direction = transform.parent.forward + spreadDirection;

        // Raycast to simulate shooting
        if (Physics.Raycast(transform.parent.position, direction, out hit))
        {
            if (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("BossEnemy"))
            {
                // If an enemy was shot, decrease its health
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

        // Instantiate bullet hole graphic at hit point and set canshoot to true
        GameObject bulletHole = Instantiate(bulletHoleGraphic, hit.point, Quaternion.LookRotation(hit.normal));
        bulletHole.transform.parent = hit.transform;
        Destroy(bulletHole, 0.5f);
        canShoot = true;
    }

    // Coroutine for reloading
    IEnumerator Reload()
    {
        isReloading = true; // Set isReloading to true
        canShoot = false; // Set canShoot to false

        // Play reload sound effect
        FindObjectOfType<AudioManager>().PlaySound("Reload");

        // Show reload UI text and play animation
        reloadText.SetActive(true);
        animator.SetBool("isReloading", true);

        // Reduce player speed while reloading
        playerController.speed = 3.0f;

        // Wait for the reload time
        yield return new WaitForSeconds(reloadTime);

        // Hide reload UI text and animation
        reloadText.SetActive(false);
        animator.SetBool("isReloading", false);

        // Determine the amount of ammo to reload
        int ammoToReload = Mathf.Min(magazineSize - currentAmmo, ammoInReserve);

        // Subtract the reloaded ammo from the reserve and add to current ammo
        currentAmmo += ammoToReload;
        ammoInReserve -= ammoToReload;

        // Set isReloading to false and canShoot to true
        isReloading = false;
        canShoot = true;

        // Restore player speed after reloading
        playerController.speed = 5.0f;
    }
}
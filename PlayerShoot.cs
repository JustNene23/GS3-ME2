using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    //public GameObject bulletPrefab; //prefab for bullets
    public Transform bulletSpawnPoint; //spawn point for bullets
    public float bulletSpeed; //bullet speed
    public ObjectPooler objectPooler;

    public float shootingRange = 100f; // Maximum distance to shoot
    public Camera playerCamera; // Reference to the player's camera

    public SoundManager soundManager;
    //reference the zombie damage script
    //public ZombieDamage zombieDamage;

    public ZombieSpawner zombieSpawner;

    public bool busyReloading = false;
    public Scope scopeScript;

    //ammo stuff
    public UpgradeSystem upgradeSystem;
    //public Text ammoCountText;
    //public Text magSizeText;
    //public Text playerLevelText;
    public int zombiesKilledLevel = 0;
    public int totalZombiesKilled = 0;

    public UpgradeManager upgradeManager;

    public int zombieWaveKillRequirement = 20;

    // Start is called before the first frame update
    void Update()
    {
        if (upgradeManager.waveBreak == false && (upgradeSystem.currentAmmo != 0 || upgradeSystem.magSize != 0))
        {
            if (Input.GetButtonDown("Fire1") && !busyReloading && (upgradeSystem.currentAmmo != 0 || upgradeSystem.magSize != 0))
            {
                Shoot();
                soundManager.shootingSound.Play();
            }

            if (busyReloading == false && Input.GetKeyDown(KeyCode.R) && (upgradeSystem.magSize != upgradeSystem.maxMagSize))
            {
                StartCoroutine(ReloadGun());
            }

            if (!busyReloading && upgradeSystem.magSize == 0 && upgradeSystem.currentAmmo > 0)
            {
                StartCoroutine(ReloadGun());
            }
        }


        if (totalZombiesKilled == zombieWaveKillRequirement && !upgradeManager.timerIsRunning)
        {
            upgradeManager.StartCountdown();
        }

        if (upgradeSystem.currentAmmo > upgradeSystem.maxAmmoCapacity)
        {
            upgradeSystem.currentAmmo = upgradeSystem.maxAmmoCapacity;
        }

    }

    

    public void PlayerLevelCalculations()
    {
        upgradeSystem.playerLevel++;
        upgradeManager.skillPointsAvailable++;
        zombiesKilledLevel = 0;
    }

    void Shoot()
    {
        // Perform a raycast to detect hit (same as before)
        Ray ray = new Ray(bulletSpawnPoint.position, bulletSpawnPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Handle hit logic (e.g., damage)
            if (hit.collider.CompareTag("Zombie"))
            {
                // Get the Zombie component and call TakeDamage
                ZombieBehaviour zombieScript = hit.collider.GetComponent<ZombieBehaviour>();
                if (zombieScript != null)
                {
                    zombieScript.isDamaged = true; // You can replace 100 with your desired damage value
                    zombieSpawner.zombiesInWave--;

                    if (zombieScript.hasAttackedPlayer == true)
                    {
                        zombieSpawner.zombiesOnPlayer--;
                    }

                    if (zombieScript.hasBeenRunning == true)
                    {
                        zombieSpawner.zombiesRunning--;
                    }
                }

                totalZombiesKilled++;

                if (zombiesKilledLevel < upgradeSystem.xpKillRequirement)
                {
                    zombiesKilledLevel++;
                }
                else if (zombiesKilledLevel == upgradeSystem.xpKillRequirement)
                {
                    PlayerLevelCalculations();
                }
                    
                
                
            }

            // Get a bullet from the object pool
            GameObject bullet = objectPooler.GetPooledBullet();

            // Activate and position the bullet
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.transform.rotation = bulletSpawnPoint.rotation;

            // Rotate 90 degrees around the X-axis to align the capsule horizontally
            bullet.transform.Rotate(90, 0, 0);
            bullet.SetActive(true);

            // Move the bullet to the hit point
            StartCoroutine(MoveBullet(bullet, hit.point));

            if (upgradeSystem.currentAmmo > 0)
            {
                upgradeSystem.currentAmmo--;
            }

            if (upgradeSystem.magSize > 0)
            {
                upgradeSystem.magSize--;
            }
            
        }
    }

    IEnumerator MoveBullet(GameObject bullet, Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - bullet.transform.position).normalized;
        float distance = Vector3.Distance(bullet.transform.position, targetPosition);

        float traveledDistance = 0f;
        while (traveledDistance < distance)
        {
            float step = bulletSpeed * Time.deltaTime;
            bullet.transform.position += direction * step;
            traveledDistance += step;

            yield return null;
        }

        // Deactivate the bullet once it reaches the target
        bullet.SetActive(false);
    }

    IEnumerator ReloadGun()
    {
        busyReloading = true;

        scopeScript.StartReload();
        
        yield return new WaitForSeconds(1);

        busyReloading = false;

        scopeScript.FinishReload();
    }
}

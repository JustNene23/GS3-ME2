using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public bool waveBreak = false;

    //wave countdown
    public Text timerText;
    public float waveInterval = 30f;
    private float waveTimeRemaining;
    public bool timerIsRunning = false;

    public GameObject weaponHolder;
    public GameObject hipfireScope;

    public Transform safeBreakArea;
    public GameObject playerObject;
    public PlayerControllerScript playerController;

    public MoveCamera moveCamera;
    public ZombieSpawner zombieSpawner;
    public PlayerShoot playerShoot;


    //upgrades Ui
    public Text ammoCountText;
    public Text magSizeText;
    public Text playerLevelText;
    public UpgradeSystem upgradeSystem;
    public int skillPointsAvailable;
    public Text skillPointsText;

    public AudioClip menuClickSound;
    public AudioSource AudioSource;

    void Start()
    {
        upgradePanel.SetActive(false);
        waveTimeRemaining = waveInterval;  // Initialize wave time remaining
        skillPointsAvailable = upgradeSystem.playerLevel;
    }

    
    void Update()
    {
        DisplayAmmoCount();
        DisplayMagSize();
        DisplayPlayerLevel();

        if (waveBreak == true)
        {
            upgradePanel.SetActive(true);
            weaponHolder.SetActive(false);
            hipfireScope.SetActive(false);

            playerController.moveSpeed = 0;
            moveCamera.sensX = 0;
            moveCamera.sensY = 0;

            skillPointsText.text = "Skill Points Available: " + skillPointsAvailable;

            if (timerIsRunning == true)
            {
                if (waveTimeRemaining > 0)
                {
                    waveTimeRemaining -= Time.deltaTime;
                    UpdateTimerDisplay(waveTimeRemaining);
                }
                else
                {
                    waveTimeRemaining = 0;
                    StopCountdown();
                    upgradePanel.SetActive(false);

                    playerShoot.zombieWaveKillRequirement += 5;
                    playerShoot.totalZombiesKilled = 0;
                    upgradeSystem.currentPlayerHealth = upgradeSystem.maxPlayerHealth;

                    //trigger next wave
                    zombieSpawner.NextWave();
                }
            }

           
        }
        else 
        {
            upgradePanel.SetActive(false);
            weaponHolder.SetActive(true);
            playerController.moveSpeed = 5;
            moveCamera.sensX = 50f;
            moveCamera.sensY = 50f;
            //hipfireScope.SetActive(true);
        }

    }

    public void BuyMagSizeUpgrade()
    {
        if (skillPointsAvailable >= 2)
        {
            skillPointsAvailable -= 2;
            upgradeSystem.maxMagSize += 2;
            AudioSource.PlayOneShot(menuClickSound);
        }
        
    }

    public void BuyAmmocapacityUpgrade()
    {
        if (skillPointsAvailable >= 2)
        {
            skillPointsAvailable -= 2;
            upgradeSystem.maxAmmoCapacity += 10;
            AudioSource.PlayOneShot(menuClickSound);
        }
    }

    public void BuyMaxHealthUpgrade()
    {
        if (skillPointsAvailable >= 2)
        {
            skillPointsAvailable -= 2;
            upgradeSystem.maxPlayerHealth += 10;
            AudioSource.PlayOneShot(menuClickSound);
        }
    }

    public void BuyAmmoPack()
    {
        if (skillPointsAvailable > 0)
        {
            skillPointsAvailable -= 1;
            upgradeSystem.currentAmmo = upgradeSystem.maxAmmoCapacity;
            AudioSource.PlayOneShot(menuClickSound);
        }
    }

    public void StartCountdown()
    {
        waveTimeRemaining = waveInterval;
        playerObject.transform.position = safeBreakArea.position;
        DestroyAllZombies();
        timerIsRunning = true;
        waveBreak = true;
        UpdateTimerDisplay(waveTimeRemaining); // Update display immediately when starting
    }

    public void StopCountdown()
    {
        timerIsRunning = false;
        waveBreak = false;
    }

    // Update the timer display
    private void UpdateTimerDisplay(float timeToDisplay)
    {
        timeToDisplay = Mathf.FloorToInt(timeToDisplay); // Remove decimals
        timerText.text = "Next Wave In: " + timeToDisplay;
    }

    void DestroyAllZombies()
    {
        // Find all GameObjects with the tag "Zombie"
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

        // Loop through the array and destroy each GameObject
        foreach (GameObject zombie in zombies)
        {
            Destroy(zombie);
        }
    }

    public void DisplayAmmoCount()
    {
        ammoCountText.text = "Ammo: " + upgradeSystem.currentAmmo + " / " + upgradeSystem.maxAmmoCapacity;
    }

    public void DisplayMagSize()
    {
        magSizeText.text = "Magazine: " + upgradeSystem.magSize + " / " + upgradeSystem.maxMagSize;
    }

    public void DisplayPlayerLevel()
    {
        playerLevelText.text = upgradeSystem.playerLevel.ToString();
    }

}

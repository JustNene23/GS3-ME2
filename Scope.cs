using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public Animator weaponAnimator;

    public GameObject scopeOverlay;

    public GameObject hipfireOverlay;

    public GameObject playerWeapon;

    public Camera mainCamera;

    public float scopedFOV = 15f;

    private float normalFOV;
    private bool isScoped = false;

    public SoundManager soundManager;

    public bool gunRunning = false;

    public PlayerShoot playerShoot;
    public AudioClip playerReload;
    public AudioSource audioSource;

    public UpgradeSystem upgradeSystem;
    public UpgradeManager upgradeManager;

    private int bulletsToReload = 0;
    


    private void Update()
    {
        if (upgradeManager.waveBreak == false)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                if (isScoped == false)
                {
                    StartCoroutine(EnableScope());
                    soundManager.scopeSound.Play();
                }
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                if (isScoped == true)
                {
                    StartCoroutine(DisableScope());
                }
            }

            if (gunRunning == true)
            {
                GunRunning();
            }
            else if (gunRunning == false)
            {
                GunWalking();
            }
        }
       
    }


    IEnumerator EnableScope()
    {
        weaponAnimator.SetBool("IsScoped", true);

        yield return new WaitForSeconds(0.15f);

        hipfireOverlay.SetActive(false);
        scopeOverlay.SetActive(true);
        playerWeapon.SetActive(false);

        normalFOV = mainCamera.fieldOfView;
        mainCamera.fieldOfView = scopedFOV;

        isScoped = true;
    }
    // Disable the weapon immediately, then scope in
    // Set the scope state
    // Wait for a short duration to synchronize animations
    // Save the normal FOV and apply scoped FOV
    //7 lines

    IEnumerator DisableScope()
    {
        mainCamera.fieldOfView = normalFOV;

        yield return new WaitForSeconds(0.15f);

        scopeOverlay.SetActive(false);
        hipfireOverlay.SetActive(true);
        weaponAnimator.SetBool("IsScoped", false);

        playerWeapon.SetActive(true);

        isScoped = false;

    }
    // Reset the FOV before re-enabling the weapon
    // Wait for a short duration to synchronize animations
    // Set the scope state
    // Re-enable the weapon
    //6 lines

    private void GunRunning()
    {
        weaponAnimator.SetBool("IsRunning", true);
        
    }

    private void GunWalking()
    {
        
        weaponAnimator.SetBool("IsRunning", false);
    }

    public void StartReload()
    {
        weaponAnimator.SetBool("IsReloading", true);
        audioSource.PlayOneShot(playerReload);
        bulletsToReload = upgradeSystem.maxMagSize - upgradeSystem.magSize;
        upgradeSystem.currentAmmo -= bulletsToReload;
    }

    public void FinishReload()
    {
        weaponAnimator.SetBool("IsReloading", false);
        bulletsToReload = 0;
        upgradeSystem.magSize = upgradeSystem.maxMagSize;
    }

}

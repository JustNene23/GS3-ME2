using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;
    //public Rigidbody playerRB;
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f; // Multiplier for sprinting


    //new stuff
    public CharacterController characterController;

    //new
    // Gravity settings
    private float gravity = -9.81f; // Gravity value //-9.81
    private float verticalVelocity = 0f; // Velocity in the Y direction
    public float gravityMultiplier = 2f; // Adjust to change the effect of gravity //2
    private bool isGrounded;
    
    public float jumpHeight = 2f; // Jump height //2
    private bool shiftPressed; // Track whether the player is sprinting

    public SoundManager soundManager;

    //player health
    public Slider healthSlider;
    //public float playerHealth = 30f;
    public ZombieSpawner zombieSpawner;
    public float damageAmount = 1f;

    public AudioSource jumpSound;

    public Scope scopeScript;
    public int currentWave;

    public UpgradeSystem upgradeSystem;

    void Awake()
    {
        controls = new PlayerControls(); // Initialize the input controls

        // Register input action callbacks
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Jump action
        controls.Player.Jump.performed += _ => Jump();

        // Sprint action
        controls.Player.Sprint.performed += _ => shiftPressed = true;
        controls.Player.Sprint.canceled += _ => shiftPressed = false;

        StartCoroutine(DamagePlayer());
        upgradeSystem.ResetStats();
    }

   

    void OnEnable()
    {
        controls.Player.Enable(); // Enable the input actions when the object is enabled
       
    }

    void OnDisable()
    {
        controls.Player.Disable(); // Disable the input actions when the object is disabled
        
    }

    void Update()
    {
        MovePlayer();
        WaveNumber();

        if (upgradeSystem.maxPlayerHealth != 30)
        {
            healthSlider.maxValue = upgradeSystem.maxPlayerHealth;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpSound.Play();
        }
    }

    public void UpdateHealthSlider()
    {
        if (upgradeSystem.currentPlayerHealth > 0)
        {
            healthSlider.value = upgradeSystem.currentPlayerHealth;
        }
       
    }

    private IEnumerator DamagePlayer()
    {
        while (upgradeSystem.currentPlayerHealth > 0)
        {
            upgradeSystem.currentPlayerHealth -= (zombieSpawner.zombiesOnPlayer * damageAmount);
            UpdateHealthSlider();
            yield return new WaitForSeconds(1f);
        }
       
    }
   

    private void MovePlayer()
    {
        // Ground check
        isGrounded = characterController.isGrounded;

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Small negative value to keep grounded
        }

        // Calculate movement
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);

        // Apply sprinting
        float currentMoveSpeed = shiftPressed ? moveSpeed * sprintMultiplier : moveSpeed;

        if (isGrounded && characterController.velocity.magnitude > 0.1f) // Check if the player is moving
        {
            if (shiftPressed)
            {
                scopeScript.gunRunning = true;
               
            }
            else if (!shiftPressed)
            {
                scopeScript.gunRunning = false;
                
            }

            if (Time.time >= soundManager.nextStepTime)  // Only play footsteps at the right interval
            {
                if (shiftPressed)
                {
                    soundManager.RunningFootsteps();
                }
                else
                {
                    soundManager.WalkingFootsteps();
                }
            }
        }

        

        // Apply gravity
        if (!isGrounded)
        {
            verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;
        }

        move.y = verticalVelocity;

        // Determine movement speed
        //float currentMoveSpeed = moveSpeed; // Default speed

        // Call SprintPlayer to determine if the player is sprinting
        //SprintPlayer(ref currentMoveSpeed); // Pass currentMoveSpeed by reference to modify it


        // Move the character controller
        characterController.Move(move * currentMoveSpeed * Time.deltaTime);

    }

    private void WaveNumber()
    {
        currentWave = zombieSpawner.currentWave;

        if (currentWave == 1)
        {
            damageAmount = 1f;
        }
        else if (currentWave == 2)
        {
            damageAmount = 1.5f;
        }
        else if (currentWave == 3)
        {
            damageAmount = 2f;
        }
    }

    /*
    private void SprintPlayer( ref float currentMoveSpeed)
    {
        if (playerStamina > 0 && shiftPressed) // Check if player can sprint
        {
            // Start stamina depletion if not already running
            if (staminaCoroutine == null)
            {
                staminaCoroutine = StartCoroutine(DepleteStamina());
            }

            // Set sprinting speed
            currentMoveSpeed = moveSpeed * sprintMultiplier;

            // Play running footsteps
            if (isGrounded && Time.time >= soundManager.nextStepTime)
            {
                soundManager.RunningFootsteps();
            }
        }
        else
        {
            // Stop stamina depletion when not sprinting
            if (staminaCoroutine != null)
            {
                StopCoroutine(staminaCoroutine);
                staminaCoroutine = null; // Reset the reference
            }

            // Start stamina regeneration if stamina is not full
            if (playerStamina < maxPlayerStamina)
            {
                // Ensure the regeneration coroutine is not already running
                if (staminaCoroutine == null)
                {
                    staminaCoroutine = StartCoroutine(RegenStamina());
                }
            }

            // Set walking speed
            currentMoveSpeed = moveSpeed;

            // Play walking footsteps
            if (isGrounded && characterController.velocity.magnitude > 0.1f && Time.time >= soundManager.nextStepTime)
            {
                soundManager.WalkingFootsteps();
            }
        }
    }
    */

   

    /*
    private void ChangeStaminaBar()
    {
        staminaBar.value = playerStamina;
    }

    private IEnumerator DepleteStamina()
    {
        while (playerStamina > 0)
        {
            playerStamina -= staminaDecreaseRate;
            ChangeStaminaBar();
            yield return new WaitForSeconds(1f);
        }

        // When stamina reaches 0, stop sprinting and handle state changes
        playerStamina = 0; // Ensure stamina does not go negative
        canSprint = false;
        //StopCoroutine(RegenStamina()); // Ensure regen coroutine is stopped

        // When stamina reaches 0, stop sprinting
        //canSprint = false;
    }


    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(staminaRegenDelay);

        while (playerStamina < maxPlayerStamina)
        {
            playerStamina += staminaRegenRate;

            if (playerStamina > maxPlayerStamina)
            {
                playerStamina = maxPlayerStamina;
            }

            ChangeStaminaBar();
            yield return new WaitForSeconds(1f);
        }

        staminaCoroutine = null; // Reset the coroutine reference once done
    }
    */
}

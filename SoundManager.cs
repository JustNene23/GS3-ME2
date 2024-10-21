using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SoundManager : MonoBehaviour
{
    public AudioSource shootingSound;

    public AudioSource scopeSound;

    public AudioClip[] runningSounds;
    public AudioClip[] walkingSounds;
    public AudioSource audioSource; //script managers audio source for playing array sounds

    public CharacterController characterController; //player reference

    public float stepInterval = 0.5f;  // Time between footsteps
    //public float walkSpeed = 3f;       // Walking speed threshold
    //public float runSpeed = 6f;        // Running speed threshold
    public float nextStepTime = 0f;

    
    public AudioClip[] zombieAttack;
    public AudioClip[] zombieRun;

    public float attackinterval = 0.5f;
    public float nextAttackSound = 0;
    public float runningInterval = 2f;
    public float nextRunningSound = 0;

    private void Start()
    {
        
    }

    void Update()
    {
        
    }

   

    public void WalkingFootsteps()
    {
        stepInterval = 0.5f;

        if (walkingSounds.Length > 0)
        {
            int index = Random.Range(0, walkingSounds.Length);
            audioSource.PlayOneShot(walkingSounds[index]);
        }
        nextStepTime = Time.time + stepInterval;
    }

    public void RunningFootsteps()
    {
        stepInterval = 0.3f;

        if (runningSounds.Length > 0)
        {
            int index = Random.Range(0, runningSounds.Length);
            audioSource.PlayOneShot(runningSounds[index]);
        }
        nextStepTime = Time.time + stepInterval;
    }

    public void ZombieAttack()
    {
        
            if (zombieAttack.Length > 0)
            {
                int index = Random.Range(0, zombieAttack.Length);
                audioSource.PlayOneShot(zombieAttack[index]);
            }
        

        nextAttackSound = Time.time + attackinterval;
    }

    public void ZombieRunning()
    {
       
            if (zombieRun.Length > 0)
            {
                int index = Random.Range(0, zombieRun.Length);
                audioSource.PlayOneShot(zombieRun[index]);
            }
        

        nextRunningSound = Time.time + runningInterval;
    }
}

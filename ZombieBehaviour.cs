using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    public Animator zombieAnimator;
    public Transform player;
    public float zombieSpeed = 3f;
    public float stoppingDistance = 1.5f;

    public bool isDamaged = false;
    public bool hasAttackedPlayer = false;
    public bool hasBeenRunning = false;

    public ZombieSpawner zombieSpawner;

    public int currentWave;

    private void Start()
    {
        if (currentWave == 1)
        {
            zombieSpeed = 3f;
        }
        else if (currentWave == 2)
        {
            zombieSpeed = 3.5f;
        }
        else if (currentWave == 3)
        {
            zombieSpeed = 4f;
        }
    }

    public void Update()
    {
        MoveZombie();
        DamageZombie();
    }

    private void MoveZombie()
    {
        // Check if the player is assigned
        if (player != null && isDamaged == false)
        {
            

            // Calculate the direction to the player
            Vector3 direction = (player.position - transform.position).normalized;
            float distance = Vector3.Distance(player.position, transform.position);

            // Move the zombie towards the player if the distance is greater than stopping distance
            if (distance > stoppingDistance)
            {
                // Move the zombie
                transform.position += direction * zombieSpeed * Time.deltaTime;
                zombieAnimator.SetBool("IsZombieRunning", true);
                zombieAnimator.SetBool("IsZombieAttacking", false);

                //zombieSpawner.zombiesRunning++;

                if (hasAttackedPlayer)
                {
                    zombieSpawner.zombiesOnPlayer--;
                    hasAttackedPlayer = false;
                }

                if (!hasBeenRunning)
                {
                    zombieSpawner.zombiesRunning++;
                    hasBeenRunning = true;
                }
            }
            else if (distance < stoppingDistance)
            {
                if (!hasAttackedPlayer)
                {
                    zombieSpawner.zombiesOnPlayer++;
                    hasAttackedPlayer = true;
                }

                if (hasBeenRunning)
                {
                    zombieSpawner.zombiesRunning--;
                    hasBeenRunning = false;
                }

                zombieAnimator.SetBool("IsZombieAttacking", true);
                zombieAnimator.SetBool("IsZombieRunning", false);

                //zombieSpawner.zombiesRunning--;
            }

            // Sample the terrain height to keep the zombie on the ground
            Terrain terrain = Terrain.activeTerrain;
            if (terrain != null)
            {
                // Update the zombie's height based on the terrain
                float terrainHeight = terrain.SampleHeight(transform.position) + terrain.transform.position.y;
                transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);
            }

            // Optionally, rotate the zombie to face the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Ensure the zombie remains upright
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); // Keep only the Y rotation
        }
    }

    
    
    public void DamageZombie()
    {
        if (isDamaged == true)
        {
            zombieAnimator.SetBool("IsZombieDamaged", true);
            //Destroy(this.gameObject);

            zombieSpawner.ZombieDied();

            StartCoroutine(ZombieDestroyed());
        }
        //zombieAnimator.SetBool("IsZombieDamaged", true);
    }
    
    private IEnumerator ZombieDestroyed()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
   

}

    


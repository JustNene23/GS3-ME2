using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UpgradeSystem", menuName = "Upgrade System", order = 1)]
public class UpgradeSystem : ScriptableObject
{
    //ammo and reloading 
    public int maxAmmoCapacity = 55; //boost by 8 per point (get 25 ammo for each wave) is displayed in Ui 
    public int currentAmmo = 55;
    public int magSize = 8; //how many bullets in magazine currently
    public int maxMagSize = 8; //is displayed in UI boost by 2 per point

    //health capacity
    public int maxPlayerHealth = 30; //boost by 10 per point
    public float currentPlayerHealth = 30;

    //vampiric and regen stuff >>>>>>>
    public float lifeStealChance = 0.25f;

    //one-time use items >>>>>>>
    public int medKitAmount = 10;
    public int ammoPack = 15;

    //skill points and levelling up
    //level up 1 time for every 7 zombies killed
    //every time you level up, add 1 to the zombie killed amount required to level up
    public int playerLevel = 0;
    public int xpKillRequirement = 5;

    //skill up prices
    public int ammoCapacityBoostCost = 2;
    public int magSizeBoostCost = 2;
    public int maxHealthBoostCost = 2;
    //public int vampiricAbilityCost = 3;
    //public int medKitCost = 1;
    //public int ammoPackCost = 1;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void ResetStats()
    {
        maxAmmoCapacity = 55;
        currentAmmo = 55;
        magSize = 8;
        maxMagSize = 8;
        maxPlayerHealth = 30;
        currentPlayerHealth = 30;
        xpKillRequirement = 5;
        playerLevel = 0;
    }

    

}

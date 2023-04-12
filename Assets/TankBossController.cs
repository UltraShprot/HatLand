using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBossController : MonoBehaviour
{
    [SerializeField] triggerSpawn triggerSpawn;
    [SerializeField] public bool isActive;
    [SerializeField] TankBossLogic tankBoss;
    [SerializeField] TankBossWheel bossWheel1;
    [SerializeField] TankBossWheel bossWheel2;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] GameObject DeadCollider;
    bool forward;
    bool invoked;
    void Start()
    {
        
    }

    void Update()
    {
        if (triggerSpawn.y && !isActive)
        {
            isActive = true;
        }
        if (isActive)
        {
            tankBoss.Life();
            if (tankBoss.Life())
            {
                gameObject.tag = "Enemy";
                tankBoss.Shoot();
                if (bossWheel1.isDead && bossWheel2.isDead)
                {
                    tankBoss.down = true;
                }
                if (forward)
                {
                    tankBoss.SetDestination(pointA);
                    bossWheel1.forward = true; 
                    bossWheel2.forward = true;
                    if (tankBoss.SetDestination(pointA) && !invoked)
                    {
                        invoked= true;
                        Invoke("Forward_false", 3f);
                    }
                }
                else
                {
                    tankBoss.SetDestination(pointB);
                    bossWheel1.forward = false;
                    bossWheel2.forward = false;
                    if (tankBoss.SetDestination(pointB) && !invoked)
                    {
                        invoked = true;
                        Invoke("Forward_true", 3f);
                    }
                }
                
            }
            else
            {
                DeadCollider.SetActive(false);
                gameObject.tag = "Untagged";
                if (SteamManager.Initialized)
                {
                    Steamworks.SteamUserStats.SetAchievement("TANK");
                    Steamworks.SteamUserStats.StoreStats();
                }
               
            }

        }
    }
    void Forward_false()
    {
        invoked = false;
        forward = false;
    }
    void Forward_true()
    {
        forward = true;
        invoked = false;
    }
}

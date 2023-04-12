using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBossController : MonoBehaviour
{
    [SerializeField] private LavaBossLogic LavaBoss;
    private Character character;
    [SerializeField] private float lives;
    [SerializeField] private float speed;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip clipDie;
    [SerializeField] private AudioClip damageAudio;
    [SerializeField] private GameObject Body;
    [SerializeField] private GameObject DeadBody;
    public bool isActive;
    States state = States.None; 
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character").GetComponent<Character>();
        LavaBoss.character= character;
        LavaBoss.lives = this.lives;
        LavaBoss.parentTransform = this.transform;
        LavaBoss.speed = this.speed;
        LavaBoss.audioSource = audioSource;
        LavaBoss.bulletPrefab= bulletPrefab;
        LavaBoss.shootPoint= shootPoint;
        LavaBoss.clip= clip;
        LavaBoss.Body = Body;
        LavaBoss.DeadBody= DeadBody;
        LavaBoss.clipDie= clipDie;
        LavaBoss.damageAudio = damageAudio;
    }

    // Update is called once per frame
    void Update()
    {
        LavaBoss.speed= speed;
        if (Vector3.Distance(character.transform.position, this.transform.position) < 20)
        {
            isActive= true;
            LavaBoss.isActive= isActive;
        }
        if (isActive)
        {
            LavaBoss.Life();
            if (LavaBoss.Life() == true)
            {
                if (LavaBoss.pain >= 10)
                {
                    LavaBoss.pain = 0;
                    state= States.Escape;
                }
                switch (state)
                {
                    case States.None:
                        state = States.Attack;
                        break;
                    case States.Attack:
                        LavaBoss.Advance();
                        LavaBoss.Shoot();
                        if (LavaBoss.Advance() == true)
                        {
                            state = States.Retreat;
                        }
                        LavaBoss.speed = speed;
                        break; 
                    case States.Retreat:
                        LavaBoss.Retreat();
                        if (LavaBoss.Retreat() == true)
                        {
                            state = States.Attack;
                        }
                        LavaBoss.speed = speed * 1.2f;
                        break;
                    case States.Escape:
                        LavaBoss.Evade();
                        if (LavaBoss.Evade() == true)
                        {
                            state = States.Retreat;
                        }
                        LavaBoss.speed = speed * 0.35f;
                        break;
                } 
                    
                
            }
            else
            {
                state = States.None;
                if (SteamManager.Initialized)
                {
                    Steamworks.SteamUserStats.SetAchievement("LAVABOSS");
                    Steamworks.SteamUserStats.StoreStats();
                }
                
                Destroy(this.gameObject, 10f);
            }

        }
    }
    enum States
    {
        None,
        Attack,
        Retreat,
        Escape, 
    }

}

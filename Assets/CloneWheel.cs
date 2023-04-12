using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class CloneWheel : MonoBehaviour
{
    [SerializeField] public AudioClip _hity;
    [SerializeField] public AudioClip dieclip;
    [SerializeField] public AudioClip damageAudio;
    public NavMeshAgent navMeshAgent;
    [SerializeField] GameObject character;
    [SerializeField] float damage;
    [SerializeField] Transform parentTransform;
    [SerializeField] public float speed;
    [SerializeField] public float lives;
    [SerializeField] AudioSource hit;
    [SerializeField] AudioSource soundoftw;
    [SerializeField] Animator animator;
    [SerializeField] GameObject DeathTexture;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] GameObject light;
    [SerializeField] MeshCollider meshCollider;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] CapsuleCollider capsuleCollider;
    float distance = 99999;
    [SerializeField] GameObject Enemy;
    GameObject LateEnemy;
    bool hitted;
    public bool isDead;
    float timer;
    [SerializeField] float lifeTime = 20f;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        character = GameObject.Find("Character");
        navMeshAgent.speed = speed;
        damage = character.GetComponent<Character>().damage;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((lives <= 0 || lifeTime <= 0) && !isDead)
        {
            isDead = true;
            hit.PlayOneShot(dieclip);
            soundoftw.enabled = false;
            navMeshAgent.speed = 0;
            animator.enabled = false;
            mesh.enabled = false;
            light.SetActive(false);
            DeathTexture.SetActive(true);
            capsuleCollider.enabled = false;
            meshCollider.enabled = false;
            boxCollider.enabled = false;
            sphereCollider.enabled = false;
            Destroy(parentTransform.gameObject, 10f);
            navMeshAgent.enabled = false;
        }
        if (!isDead)
        {
            StayOrGo();
            FindEnemy();
            GoAndAttackEnemy();
        }
    }
    void StayOrGo()
    {
        if (!isDead)
        {
            if (Enemy == null)
            {
                if (Vector3.Distance(transform.position, character.transform.position) > 6)
                {
                    navMeshAgent.SetDestination(character.transform.position);
                    navMeshAgent.speed = speed;
                }
                else
                {
                    navMeshAgent.speed = 0;
                }
            }
        }
    }
    void FindEnemy()
    {
        GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (AllEnemies.Length > 0 && Enemy == null)
        {
            foreach (GameObject AllEnemiesHPNotNull in AllEnemies)
            {
                try
                {
                    if (AllEnemiesHPNotNull.GetComponent<DEnemy>().lives > 0)
                    {

                        if (distance > Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position))
                        {
                            distance = Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position);
                            LateEnemy = AllEnemiesHPNotNull;
                            continue;
                        }

                    }
                }
                catch { }
                try
                {
                    if (AllEnemiesHPNotNull.GetComponent<BEnemy>().lives > 0)
                    {
                        if (distance > Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position))
                        {
                            distance = Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position);
                            LateEnemy = AllEnemiesHPNotNull;
                            continue;
                        }
                    }
                }
                catch { }
                try
                {
                    if (AllEnemiesHPNotNull.GetComponent<TNTEnemy>().lives > 0)
                    {
                        if (distance > Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position))
                        {
                            distance = Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position);
                            LateEnemy = AllEnemiesHPNotNull;
                            continue;
                        }
                    }
                }
                catch { }
                try
                {
                    if (AllEnemiesHPNotNull.GetComponent<GBoss>().lives > 0)
                    {
                        if (distance > Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position))
                        {
                            distance = Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position);
                            LateEnemy = AllEnemiesHPNotNull;
                            continue;
                        }
                    }
                }
                catch { }
                try
                {
                    if (AllEnemiesHPNotNull.GetComponent<DHBoss>().lives > 0)
                    {
                        if (distance > Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position))
                        {
                            distance = Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position);
                            LateEnemy = AllEnemiesHPNotNull;
                            continue;
                        }
                    }
                }
                catch { }
                try
                {
                    if (AllEnemiesHPNotNull.GetComponent<CloneSpawnBoss>().lives > 0)
                    {
                        if (distance > Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position))
                        {
                            distance = Vector3.Distance(transform.position, AllEnemiesHPNotNull.transform.position);
                            LateEnemy = AllEnemiesHPNotNull;
                            continue;
                        }

                    }
                }
                catch { }
            }
            if (LateEnemy != null)
            {
                Enemy = LateEnemy;
            }
            distance = 999999;
        }
    }
    void GoAndAttackEnemy()
    {
        if (Enemy != null)
        {
            lifeTime -= Time.deltaTime;
            try
            {
                if (Enemy.GetComponent<DEnemy>().lives <= 0)
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (Enemy.GetComponent<BEnemy>().lives <= 0)
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (Enemy.GetComponent<TNTEnemy>().lives <= 0)
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (Enemy.GetComponent<GBoss>().lives <= 0)
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (Enemy.GetComponent<DHBoss>().lives <= 0)
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (Enemy.GetComponent<CloneSpawnBoss>().lives <= 0)
                {
                    Enemy = null;
                }
            }
            catch { }
            if (Enemy != null)
            {
                navMeshAgent.SetDestination(Enemy.transform.position);
                navMeshAgent.speed = speed;
            }
        }
    }
    private void OnTriggerStay(Collider collusion)
    {
        if (collusion.gameObject.tag == "Enemy" && character.GetComponent<Character>().lives > 0 && !isDead && timer > 0.5f)
        {
            timer = 0f;
            try
            {
                if (collusion.GetComponent<DEnemy>().lives > 0)
                {
                    collusion.GetComponent<DEnemy>().lives -= damage;
                    hit.PlayOneShot(_hity);
                }
                else
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (collusion.GetComponent<BEnemy>().lives > 0)
                {
                    collusion.GetComponent<BEnemy>().lives -= damage;
                    hit.PlayOneShot(_hity);
                }
                else
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (collusion.GetComponent<TNTEnemy>().lives > 0)
                {
                    collusion.GetComponent<TNTEnemy>().lives -= damage;
                    hit.PlayOneShot(_hity);
                }
                else
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (collusion.GetComponent<GBoss>().lives > 0)
                {
                    collusion.GetComponent<GBoss>().lives -= damage;
                    hit.PlayOneShot(_hity);
                }
                else
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (collusion.GetComponent<DHBoss>().lives > 0)
                {
                    collusion.GetComponent<DHBoss>().lives -= damage;
                    hit.PlayOneShot(_hity);
                }
                else
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (collusion.GetComponent<CloneSpawnBoss>().lives > 0)
                {
                    collusion.GetComponent<CloneSpawnBoss>().lives -= damage;
                    hit.PlayOneShot(_hity);
                }
                else
                {
                    Enemy = null;
                }
            }
            catch { }
            try
            {
                if (collusion.GetComponent<MiniWorm>().lives > 0)
                {
                    collusion.GetComponent<MiniWorm>().lives -= damage;
                    hit.PlayOneShot(_hity);
                }
                else
                {
                    Enemy = null;
                }
            }
            catch { }
        }

        if (collusion.gameObject.tag == "Bullet" && !isDead)
        {
            lives -= 1;
            hit.PlayOneShot(damageAudio);
        }
    }
}

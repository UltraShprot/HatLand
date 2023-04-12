using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class FTripWheel : MonoBehaviour
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
    [SerializeField] GameObject TNT;
    bool hitted;
    public bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        character = GameObject.Find("Character");
        
        navMeshAgent.speed= speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            navMeshAgent.SetDestination(character.transform.position);
        }
        if (lives <= 0 && !isDead)
        {
            isDead = true;
            hit.PlayOneShot(dieclip);
            soundoftw.enabled= false;
            navMeshAgent.speed = 0;
            animator.enabled = false;
            mesh.enabled= false;
            light.SetActive(false);
            DeathTexture.SetActive(true);
            capsuleCollider.enabled= false;
            meshCollider.enabled= false;
            boxCollider.enabled= false;
            sphereCollider.enabled= false;
            character.GetComponent<Character>().enemys += 1;
            Destroy(parentTransform.gameObject, 10f);
            navMeshAgent.isStopped = true;
            if (SteamManager.Initialized)
            {
                Steamworks.SteamUserStats.SetAchievement("FWHEEL");
                Steamworks.SteamUserStats.StoreStats();
            }
        }
        if (!isDead)
        {
            if (TNT != null)
            {
                navMeshAgent.speed = speed / 1.5f;
            }
            else
            {
                navMeshAgent.speed = speed;
            }
        }
        NavMeshHit hitNavMesh;
        navMeshAgent.SamplePathPosition(1, 1000, out hitNavMesh);
        if (hitNavMesh.mask == 4)
        {
            navMeshAgent.speed = 2.5f;
        }
        else if (hitNavMesh.mask == 1)
        {
            navMeshAgent.speed = 8;
        }
    }
    void OnTriggerEnter(Collider collusion)
    {
        if (collusion.gameObject.name == "Character" && character.GetComponent<Character>().lives > 0 && !isDead)
        {
            character.GetComponent<Character>().lives -= damage;
            hit.PlayOneShot(_hity);
        }
        if (collusion.gameObject.tag == "Bullet" && !isDead)
        {
            lives -= character.GetComponent<Character>().damage;
            hit.PlayOneShot(damageAudio);
            if (TNT != null && !hitted)
            {
                TNT.GetComponent<TNT>().exp = true;
                hitted= true;
            }
        }
    }
}

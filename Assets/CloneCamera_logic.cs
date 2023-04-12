using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class CloneCamera_logic : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSource2;
    [SerializeField] AudioClip clipDead;
    [SerializeField] AudioClip clipDetect;
    [SerializeField] AudioClip clipDetected;
    [SerializeField] AudioClip clipHit;
    [SerializeField] Light light;
    [SerializeField] GameObject detector;
    [SerializeField] public float lives = 3;
    [SerializeField] GameObject Character;
    [SerializeField] GameObject Clone;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;
    [SerializeField] Transform spawnPoint3;
    [SerializeField] Animator animator;
    [SerializeField] float time;
    [SerializeField] float time2;
    bool isDestroyed;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] int timeSpawned = 3;

    // Start is called before the first frame update
    void Start()
    {
        Character = GameObject.Find("CharacterYDynamicPoint");
        sphereCollider = GetComponent<SphereCollider>();
        mesh= GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((lives <= 0 || timeSpawned <= 0) && !isDestroyed)
        {
            isDestroyed= true;
            particleSystem.Play();
            light.enabled= false;
            audioSource.PlayOneShot(clipDead);
            mesh.enabled = false;
            sphereCollider.enabled = false;
            detector.SetActive(false);
            particleSystem.Play();
            Destroy(this.gameObject, 10f);
            audioSource2.enabled= false;
            lineRenderer.enabled = false;
            if (SteamManager.Initialized)
            {
                Steamworks.SteamUserStats.SetAchievement("CAMERA_DESTROY");
                Steamworks.SteamUserStats.StoreStats();
            }
        }
        DetectionLogic();
    }
    void DetectionLogic()
    {
        if (!isDestroyed)
        {
            if (detector.GetComponent<CloneCamera_detector>().detected)
            {
                if (time == 0) 
                {
                    audioSource.PlayOneShot(clipDetect);
                }
                time2 = 3;
                time += Time.deltaTime;
                light.color= Color.red;
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor= Color.red;
                animator.enabled= false;
                transform.LookAt(Character.transform.position);
            }
            else
            {
                if (time2 > -3)
                    time2 -= Time.deltaTime;
                if (time2 < 0)
                {
                    animator.enabled = true;
                    light.color = Color.yellow;
                    lineRenderer.startColor = Color.yellow;
                    lineRenderer.endColor = Color.yellow;
                    time = 0;
                }
            }
            if (time >= 1.7f)
            {
                audioSource.PlayOneShot(clipDetected);
                time = 0;
                timeSpawned -= 1;
                Instantiate(Clone, spawnPoint1.position, spawnPoint1.rotation);
                Instantiate(Clone, spawnPoint2.position, spawnPoint1.rotation);
                Instantiate(Clone, spawnPoint3.position, spawnPoint1.rotation);
                if (SteamManager.Initialized)
                {
                    Steamworks.SteamUserStats.SetAchievement("CAMERA_ON");
                    Steamworks.SteamUserStats.StoreStats();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" && !isDestroyed)
        {
            lives -= 1;
            audioSource.PlayOneShot(clipHit);
        }
    }
}

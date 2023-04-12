using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.AI;

public class MiniWorm : MonoBehaviour
{
    Vector3 startPosition;
    [SerializeField] public float lives = 3.25f;
    Transform character;
    System.Random rnd= new System.Random();
    System.Random rnd2 = new System.Random();
    public States state = new States();
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clipDie;
    [SerializeField] AudioClip clipShoot;
    [SerializeField] AudioClip clipDamage;
    [SerializeField] GameObject BulletPrefab;
    float timer;
    [SerializeField] float timeUpDown = 1.25f;
    [SerializeField] float timeShoot = 4f;
    [SerializeField] float timeNone = 2f;
    float StateTime;
    [SerializeField] Transform shootPoint;
    bool isDead;
    Animator animator;
    [SerializeField] float distance = 3.5f;
    void Start()
    {
        startPosition = transform.position;
        character = GameObject.Find("Character").transform;
        state = States.Up;
        TransformPosition();
        StateTime = timeUpDown;
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Life();
        Vector3 lookAt= new Vector3(character.position.x,transform.position.y ,character.position.z);
        if (!isDead)
        {
            transform.LookAt(lookAt);
            timer += Time.deltaTime;
        }
        if (timer > StateTime && !isDead)
        {
            switch (state)
            {
                case States.Up:
                    state = States.Shoot;
                    animator.SetBool("shoot", true);
                    StartCoroutine(Shoot());
                    StateTime = timeShoot;
                    break;
                case States.Shoot:
                    state= States.Down;
                    animator.SetBool("shoot", false);
                    StopAllCoroutines();
                    StateTime = timeUpDown;
                    break;
                case States.Down:
                    state = States.None;
                    animator.SetBool("none", true);
                    StateTime = timeNone;
                    Invoke("TransformPosition", 1f);
                    break; 
                case States.None:
                    state = States.Up;
                    animator.SetBool("none", false);
                    StateTime = timeUpDown;
                    break;

            }
            timer = 0;
        }
    }
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(1f);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(1f);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(1f);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(1f);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(1f);
    }
    private void TransformPosition()
    {
        float x = (float)(rnd.NextDouble() * distance);
        float z = (float)(rnd2.NextDouble() * distance);
        int random = rnd.Next(0, 3);
        switch (random) 
        {
            case 0: x *= -1;
                break;
            case 1: z *= -1;
                break;
            case 2:
                x *= -1;
                z *= -1;
                break;
        }

        Vector3 transformVector = new Vector3(x, 0, z);
        transform.position = startPosition + transformVector;
    }
    private void Life()
    {
        if (lives <= 0 && !isDead)
        {
            state = States.Down;
            animator.SetBool("none", false);
            animator.SetBool("shoot", false);
            StopAllCoroutines();
            isDead = true;
            audioSource.PlayOneShot(clipDie);
            Destroy(this.gameObject, 2f);
            if (SteamManager.Initialized)
            {
                Steamworks.SteamUserStats.SetAchievement("MINIWORM");
                Steamworks.SteamUserStats.StoreStats();
            }
        }
    }
    public enum States
    {
        None,
        Up,
        Down,
        Shoot
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" && state != States.None && !isDead)
        {
            lives -= character.GetComponent<Character>().damage;
            audioSource.PlayOneShot(clipDamage);
        }
        if (other.gameObject.tag == "Enemy" && state != States.Shoot && !isDead)
        {
            TransformPosition();
        }
    }
}

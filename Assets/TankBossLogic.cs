using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TankBossLogic : MonoBehaviour
{
    public float lives = 80f;
    public bool isDead;
    Character character;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clipDie;
    [SerializeField] AudioClip clipDamage;
    [SerializeField] AudioClip clipShoot;
    [SerializeField] Transform head;
    [SerializeField] GameObject dieHead;
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] ParticleSystem particleSystem;
    public bool down;
    [SerializeField] public float speed = 6f;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character").GetComponent<Character>();
    }
    public void Shoot()
    {
        if (time == 0)
        {
            time = 1;
            StartCoroutine(ShootCoroutine());
        }
    }
    IEnumerator ShootCoroutine()
    {
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        SwitchTime();
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        SwitchTime();
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        SwitchTime();
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        SwitchTime();
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        SwitchTime();
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        yield return new WaitForSeconds(time);
        Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.PlayOneShot(clipShoot);
        SwitchTime();
        yield return new WaitForSeconds(time);

        time = 0;
    }
    void SwitchTime()
    {
        switch (time)
        {
            case 1: time = 0.9f; break;
            case 0.9f: time = 0.8f; break;
            case 0.8f: time = 0.7f; break;
            case 0.7f: time = 0.6f; break;
            case 0.6f: time = 0.5f; break;
            case 0.5f: time = 0.3f; break;
        }
    }
    public bool SetDestination(Transform point)
    {
        if ((Vector3.Distance(transform.position, point.position) < 0.01f && !down) || (down && Vector3.Distance(transform.position, new Vector3(point.position.x, -12.25f, point.position.z)) < 0.01f))
        {
            return true;
        }
        else
        {
            if (!down)  
            {
                transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
            }
            else
            {
                Vector3 down = new Vector3(point.position.x, -12.25f, point.position.z);
                transform.position = Vector3.MoveTowards(transform.position, down, speed * Time.deltaTime);
            }
            return false;
        }

    }
    public bool Life()
    {
        if (lives <= 0 && !isDead)
        {
            isDead = true;
            audioSource.PlayOneShot(clipDie);
            StopAllCoroutines();
            dieHead.SetActive(true);
            particleSystem.Play();
            head.gameObject.SetActive(false);
            character.boses += 1;
        }
        if (!isDead)
        {
            Vector3 lookAt = new Vector3(character.transform.position.x, head.position.y, character.transform.position.z);
            head.LookAt(lookAt);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void MinusHP()
    {
        if (!isDead && down)
        {
            lives -= character.damage;
            audioSource.PlayOneShot(clipDamage);
        }
    }
}

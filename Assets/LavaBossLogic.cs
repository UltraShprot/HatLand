using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class LavaBossLogic : MonoBehaviour
{
    public Character character;
    public float lives;
    public bool isDead;
    public float speed;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    float shootTimer;
    public Transform parentTransform;
    public AudioSource audioSource;
    public AudioClip clip;
    public AudioClip clipDie;
    public AudioClip damageAudio;
    public GameObject Body;
    public GameObject DeadBody;
    public int pain;
    float attackTimer;
    MeshCollider meshCollider;
    public bool isActive;
    private void Start()
    {
        meshCollider= GetComponent<MeshCollider>();
    }
    public void Shoot()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > 0 && !isDead && Vector3.Distance(transform.position, character.transform.position) > 4f) 
        {
            shootTimer= -1f;
            Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            audioSource.PlayOneShot(clip);
        }
    }
    public bool Evade() 
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(parentTransform.position.x, parentTransform.position.y - 5, parentTransform.position.z), speed * Time.deltaTime);
        transform.LookAt(parentTransform.position);
        if (transform.position == new Vector3(parentTransform.position.x, parentTransform.position.y - 5, parentTransform.position.z)) {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool Retreat()
    {
        transform.position = Vector3.MoveTowards(transform.position, parentTransform.position, speed * Time.deltaTime);
        transform.LookAt(parentTransform.position);
        if (transform.position == parentTransform.position)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool Advance()
    {
        transform.position = Vector3.MoveTowards(transform.position, character.transform.position, speed * Time.deltaTime);
        transform.LookAt(character.transform.position);
        if (Vector3.Distance(transform.position, character.transform.position) < 2.95f && attackTimer > 1.85f)
        {

            attackTimer= 0;
            return true;
        }
        else
        {
            attackTimer += Time.deltaTime;
            return false;
        }
    }
    public bool Life()
    {
        if (lives <= 0 && !isDead)
        {
            isDead = true;
            audioSource.PlayOneShot(clipDie);
            Body.SetActive(false);
            DeadBody.SetActive(true);
            meshCollider.enabled = false;
        }
        if (!isDead)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" && lives > 0 && isActive)
        {
            lives -= character.damage;
            pain += 1;
            audioSource.PlayOneShot(damageAudio);
        }
    }
}

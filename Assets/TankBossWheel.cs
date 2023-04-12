using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBossWheel : MonoBehaviour
{
    public bool forward;
    [SerializeField] float _lives = 40f;
    public bool isDead;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] ParticleSystem _particleSystem2;
    [SerializeField] Animator _animator;
    [SerializeField] MeshCollider _meshCollider;
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] MeshRenderer _meshRenderer2;
    [SerializeField] AudioClip clipDie;
    [SerializeField] AudioClip clipDamage;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSource2;
    Character character;
    private void Start()
    {
        character = GameObject.Find("Character").GetComponent<Character>();
    }
    void Update()
    {
        if (forward)
        {
            _animator.SetBool("forward", true);
        }
        else
        {
            _animator.SetBool("forward", false);
        }
        if (_lives <= 0 && !isDead)
        {
            isDead = true;
            _meshCollider.enabled = false;
            _meshRenderer.enabled = false;
            _meshRenderer2.enabled = false;
            audioSource2.clip = null;
            audioSource.PlayOneShot(clipDie);
            _particleSystem.Play();
            _particleSystem2.Play();
            _animator.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" && !isDead)
        {
            _lives -= character.damage;
            audioSource.PlayOneShot(clipDamage);
        }
    }
}

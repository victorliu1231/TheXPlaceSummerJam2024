using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float speed;
    public float lifetime;
    public ParticleSystem hitParticles;
    public float hitParticleLifetime;
    public bool canCauseKnockback = true;
    private SpriteRenderer spriteRenderer;

    void Start(){
        Destroy(gameObject, lifetime);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        if (!GameManager.Instance.isGameOver){
            // Move transform based on rotation
            transform.position += transform.right * speed * Time.deltaTime;
        } else {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall"){
            if (other.gameObject.tag == "Enemy") other.gameObject.GetComponent<Enemy>().TakeDamage(damage, transform.position, this);
            if (hitParticles != null) hitParticles.Play();
            spriteRenderer.enabled = false;
            Destroy(gameObject, hitParticleLifetime);
        }
    }
}
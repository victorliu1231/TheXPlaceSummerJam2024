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
    private SpriteRenderer spriteRenderer;

    void Start(){
        Destroy(gameObject, lifetime);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall"){
            if (other.gameObject.tag == "Enemy") other.gameObject.GetComponent<Enemy>().TakeDamage(damage, transform.position);
            if (hitParticles != null) hitParticles.Play();
            spriteRenderer.enabled = false;
            Destroy(gameObject, hitParticleLifetime);
        }
    }
}
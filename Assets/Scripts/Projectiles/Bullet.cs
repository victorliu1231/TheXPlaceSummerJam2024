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
    public float knockbackForce;
    public string targetTag;
    private SpriteRenderer spriteRenderer;

    void Start(){
        Destroy(gameObject, lifetime * Util.GetStage(GetComponent<TimeSlowdown>()));
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        if (!GameManager.Instance.isGameOver && !GameManager.Instance.inTransition){
            // Move transform based on rotation
            transform.position += transform.right * speed * Time.deltaTime * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>());
        } else {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == targetTag || other.gameObject.tag == "Wall"){
            if (other.gameObject.tag == targetTag) other.gameObject.GetComponent<Entity>().TakeDamage(damage, transform.position, canCauseKnockback, knockbackForce, gameObject);
            if (hitParticles != null) hitParticles.Play();
            spriteRenderer.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, hitParticleLifetime);
        }
    }
}
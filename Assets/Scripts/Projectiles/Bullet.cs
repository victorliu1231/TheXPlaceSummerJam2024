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
    private int playerDir;
    public bool isPlayerWeapon;

    void Start(){
        Destroy(gameObject, lifetime);
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerDir = GameManager.Instance.player.transform.localScale.x > 0 ? 1 : -1;
    }

    void Update(){
        if (!GameManager.Instance.isGameOver){
            // Move transform based on rotation
            if (isPlayerWeapon) transform.position += transform.right * playerDir * speed * Time.deltaTime;
            else transform.position += transform.right * speed * Time.deltaTime;
        } else {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == targetTag || other.gameObject.tag == "Wall"){
            if (other.gameObject.tag == targetTag) other.gameObject.GetComponent<Entity>().TakeDamage(damage, transform.position, canCauseKnockback, knockbackForce);
            if (hitParticles != null) hitParticles.Play();
            spriteRenderer.enabled = false;
            Destroy(gameObject, hitParticleLifetime);
        }
    }
}
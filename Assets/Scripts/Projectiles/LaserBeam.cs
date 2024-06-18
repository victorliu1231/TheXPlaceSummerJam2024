using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float damage;
    [Tooltip("If lifetime < 0, then the laser will not destroy itself")]
    public float lifetime;
    public GameObject hitParticlesPrefab;
    public bool canCauseKnockback = false;
    public float knockbackForce;
    public string targetTag;

    void Start(){
        if (lifetime >= 0){
            Destroy(gameObject, lifetime);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if (other.gameObject.tag == targetTag){
            other.gameObject.GetComponent<Entity>().TakeDamage(damage, transform.position, canCauseKnockback, knockbackForce);
            if (hitParticlesPrefab != null){
                GameObject particles = Instantiate(hitParticlesPrefab, other.transform.position, Quaternion.identity);
                Destroy(particles, 0.5f);
            }
        }
    }
}
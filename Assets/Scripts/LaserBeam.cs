using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float damage;
    public GameObject hitParticlesPrefab;
    public bool canCauseKnockback = false;

    void OnTriggerStay2D(Collider2D other){
        if (other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage, transform.position, canCauseKnockback);
            if (hitParticlesPrefab != null){
                GameObject particles = Instantiate(hitParticlesPrefab, other.transform.position, Quaternion.identity);
                Destroy(particles, 0.5f);
            }
        }
    }
}
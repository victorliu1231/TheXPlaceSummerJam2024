using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : Projectile
{
    public float damage;
    [Tooltip("If lifetime < 0, then the laser will not destroy itself")]
    public float lifetime;
    public GameObject hitParticlesPrefab;
    public bool canCauseKnockback = false;
    public float knockbackForce;
    public string targetTag;
    public enum DamageTriggerType {OnTriggerEnter, OnTriggerStay};
    public DamageTriggerType damageTriggerType;

    void Start(){
        if (lifetime >= 0){
            Destroy(gameObject, lifetime * Util.GetStage(GetComponent<TimeSlowdown>()));
            if (!isGhost) GetComponent<TimeSlowdown>()?.ChangeStage(GameManager.Instance.stage);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == targetTag && damageTriggerType == DamageTriggerType.OnTriggerEnter){
            other.gameObject.GetComponent<Entity>().TakeDamage(damage, transform.position, canCauseKnockback, knockbackForce, gameObject);
            if (hitParticlesPrefab != null){
                GameObject particles = Instantiate(hitParticlesPrefab, other.transform.position, Quaternion.identity);
                Destroy(particles, 0.5f);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if (other.gameObject.tag == targetTag && damageTriggerType == DamageTriggerType.OnTriggerStay){
            other.gameObject.GetComponent<Entity>().TakeDamage(damage, transform.position, canCauseKnockback, knockbackForce, gameObject);
            if (hitParticlesPrefab != null){
                GameObject particles = Instantiate(hitParticlesPrefab, other.transform.position, Quaternion.identity);
                Destroy(particles, 0.5f);
            }
        }
    }
}
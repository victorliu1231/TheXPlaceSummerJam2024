using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float speed;
    public float currentHealth;
    public float maxHealth;
    public bool invincible = false;
    public bool canTakeKnockback = true;
    public float knockbackForce;
    public ParticleSystem damagedParticles;

    public void Start(){
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, Vector2 attackerPosition){
        if (!invincible){
            currentHealth -= damage;
            if (currentHealth <= 0){
                Die();
            }
            damagedParticles.Play();
            // take knockback based on position of attacker and knockback force and modify transform.position
            if (canTakeKnockback){
                Vector2 knockbackDirection = (new Vector2(transform.position.x, transform.position.y) - attackerPosition).normalized;
                // modify position of entity instead of using rigidbody dynamics
                transform.position = new Vector3(transform.position.x + knockbackDirection.x * knockbackForce, transform.position.y + knockbackDirection.y * knockbackForce, transform.position.z);
            }
        }
    }

    void Die(){
        Destroy(gameObject);
    }
}
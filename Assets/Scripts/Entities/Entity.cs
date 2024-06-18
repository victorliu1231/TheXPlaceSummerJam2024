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
    public ParticleSystem damagedParticles;
    private MMHealthBar healthbar;

    public void Start(){
        currentHealth = maxHealth;
        if (healthbar == null) healthbar = GetComponentInChildren<MMHealthBar>();
    }

    public void TakeDamage(float damage, Vector2 attackerPosition, bool canCauseKnockback, float knockbackForce){
        if (!invincible && !GameManager.Instance.isGameOver){
            currentHealth -= damage;
            healthbar.UpdateBar(currentHealth, 0, maxHealth, true);
            if (currentHealth <= 0){
                Die();
            }
            damagedParticles.Play();
            // take knockback based on position of attacker and knockback force and modify transform.position
            if (canTakeKnockback && canCauseKnockback){
                Vector2 knockbackDirection = (new Vector2(transform.position.x, transform.position.y) - attackerPosition).normalized;
                // modify position of entity instead of using rigidbody dynamics
                transform.position = new Vector3(transform.position.x + knockbackDirection.x * knockbackForce, transform.position.y + knockbackDirection.y * knockbackForce, transform.position.z);
            }
        }
    }

    public virtual void Die(){
        Destroy(gameObject);
    }
}
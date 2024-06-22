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
    public float immunityTime = 0.4f;
    public MMHealthBar healthbar;
    public bool canReceiveDamage = true;

    public void Start(){
        currentHealth = maxHealth;
        if (healthbar == null) healthbar = GetComponentInChildren<MMHealthBar>();
        if (invincible) healthbar.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage, Vector2 attackerPosition, bool canCauseKnockback, float knockbackForce, GameObject dealer = null){
        if (canReceiveDamage){
            if (dealer.GetComponent<LaserBeam>() == null) canReceiveDamage = false; // turn on immunity time if the damage dealer is not a laser beam
            Invoke("TurnOnDamage", immunityTime);
            if (dealer is not null)
            {
                if (GetComponent<TimeSlowdown>() is not null && dealer.GetComponent<TimeSlowdown>() is not null){
                    if (GetComponent<TimeSlowdown>().stage < dealer.GetComponent<TimeSlowdown>().stage)
                    {
                        if (gameObject.tag == "Enemy") GetComponent<TimeSlowdown>().ChangeStage(dealer.GetComponent<TimeSlowdown>().stage);
                    }
                }
            }
            
            if (!GameManager.Instance.isGameOver){
                if (!invincible){
                    if (gameObject.tag == "Player"){
                        currentHealth -= damage * GameManager.Instance.enemyStrengthMultiplier;
                    } else if (gameObject.tag == "Enemy"){
                        currentHealth -= damage * GameManager.Instance.playerStrengthMultiplier;
                    }
                    healthbar.UpdateBar(currentHealth, 0, maxHealth, true);
                    if (currentHealth <= 0){
                        Die();
                    }
                }
                
                damagedParticles.Play();
                AudioManager.GetSFX("Damage")?.Play();
                
                // take knockback based on position of attacker and knockback force and modify transform.position
                if (canTakeKnockback && canCauseKnockback){
                    Vector2 knockbackDirection = (new Vector2(transform.position.x, transform.position.y) - attackerPosition).normalized;
                    // modify position using positioning up until the position specified by gamemanager's radius
                    Vector3 newPosition = transform.position + new Vector3(knockbackDirection.x, knockbackDirection.y, 0) * knockbackForce;
                    if (Vector2.Distance(Vector2.zero, newPosition) <= GameManager.Instance.clockRadius){
                        transform.position = newPosition;
                    } else {
                        transform.position = newPosition.normalized * GameManager.Instance.clockRadius;
                    }
                }
            }
        }
    }

    public void TurnOnDamage(){
        canReceiveDamage = true;
    }

    public virtual void Die(){
        Destroy(gameObject);
    }
}
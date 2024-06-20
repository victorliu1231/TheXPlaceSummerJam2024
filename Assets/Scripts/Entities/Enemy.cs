using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float attackCooldownDuration;
    public Transform target;
    public bool canCauseKnockback = true;
    public Weapon weaponInHand;
    public float distanceStartAttacking;
    private float attackCooldownTimer = 0f;
    [HideInInspector]public Animator anim;
    public string movementAnimName;
    public string attackAnimName;
    public bool isSpriteFlippable;
    public enum SpriteDirection{Right, Left};
    [Tooltip("This is the direction the sprite is facing in the import.")]
    public SpriteDirection spriteDirection = SpriteDirection.Right;
    public bool isAttackCooldownReduced = false;
    public SpriteRenderer attackCooldownSprite;
    public bool isGhost;

    public void Start(){
        base.Start();
        anim = GetComponent<Animator>();
    }

    public void BindTarget(){
        if (isGhost) {
            GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject playerObj in playerObjs){
                if (playerObj.GetComponent<TimeSlowdown>().stage == 1) {
                    target = playerObj.transform;
                }
            }
        } else {
            target = GameManager.Instance.player.transform;
        }
        weaponInHand.BindTarget(target);
    }

    public virtual void Update(){
        int stage = GameManager.Instance.stage - (GetComponent<TimeSlowdown>()?.stage ?? 1) + 1;

        if (!GameManager.Instance.isGameOver){
            if (target != null && !GameManager.Instance.inTransition){
                attackCooldownTimer += Time.deltaTime * (1f/stage);
                if (isSpriteFlippable) {
                    if (spriteDirection == SpriteDirection.Right) GetComponent<SpriteRenderer>().flipX = target.position.x < transform.position.x;
                    if (spriteDirection == SpriteDirection.Left) GetComponent<SpriteRenderer>().flipX = target.position.x > transform.position.x;
                }
                if (Vector3.Distance(transform.position, target.position) <= distanceStartAttacking){
                    if (attackCooldownTimer >= attackCooldownDuration){
                        if (anim != null) anim.Play(attackAnimName);
                        weaponInHand.Attack();
                        attackCooldownTimer = 0f;
                    }
                } else {
                    Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime * (1f / stage));
                    GetComponent<Rigidbody2D>().MovePosition(pos);
                    if (anim != null) anim.Play(movementAnimName);
                }
            }
        } else {
            gameObject.SetActive(false);
        }
    }

    public override void TakeDamage(float damage, Vector2 attackerPosition, bool canCauseKnockback, float knockbackForce, GameObject dealer = null){
        if (dealer is not null)
        {
            if (GetComponent<TimeSlowdown>().stage < dealer.GetComponent<TimeSlowdown>().stage)
            {
                GetComponent<TimeSlowdown>().ChangeStage(dealer.GetComponent<TimeSlowdown>().stage);
                if (dealer.GetComponent<TimeSlowdown>().stage >= GameManager.Instance.stage) {
                    GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    target = GameManager.Instance.player.transform;
                    weaponInHand.BindTarget(target);
                    weaponInHand.gameObject.GetComponent<TimeSlowdown>().ChangeStage(dealer.GetComponent<TimeSlowdown>().stage, true);
                }
            }
        }
        
        if (!invincible && !GameManager.Instance.isGameOver){
            if (gameObject.tag == "Player"){
                currentHealth -= damage * GameManager.Instance.enemyStrengthMultiplier;
            } else if (gameObject.tag == "Enemy"){
                currentHealth -= damage * GameManager.Instance.playerStrengthMultiplier;
            }
            healthbar.UpdateBar(currentHealth, 0, maxHealth, true);
            damagedParticles.Play();
            AudioManager.GetSFX("Damage")?.Play();
            if (currentHealth <= 0){
                Die();
            }
            // take knockback based on position of attacker and knockback force and modify transform.position
            if (canTakeKnockback && canCauseKnockback){
                Vector2 knockbackDirection = (new Vector2(transform.position.x, transform.position.y) - attackerPosition).normalized;
                // modify position of entity instead of using rigidbody dynamics
                transform.position = new Vector3(transform.position.x + knockbackDirection.x * knockbackForce, transform.position.y + knockbackDirection.y * knockbackForce, transform.position.z);
            }
        }
    }
}
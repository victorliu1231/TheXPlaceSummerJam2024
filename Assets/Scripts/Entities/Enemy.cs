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
}
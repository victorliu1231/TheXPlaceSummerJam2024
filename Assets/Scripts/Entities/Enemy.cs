using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float attackCooldownDuration;
    [HideInInspector]public Transform target;
    public bool canCauseKnockback = true;
    public Weapon weaponInHand;
    public float distanceStartAttacking;
    private float attackCooldownTimer = 0f;
    [HideInInspector]public Animator anim;
    public string movementAnimName;
    public string attackAnimName;

    public void Start(){
        base.Start();
        target = GameManager.Instance.player.transform;
        anim = GetComponent<Animator>();
    }

    public virtual void Update(){
        if (!GameManager.Instance.isGameOver){
            if (target != null && !GameManager.Instance.inTransition){
                attackCooldownTimer += Time.deltaTime;
                if (Vector3.Distance(transform.position, target.position) <= distanceStartAttacking){
                    if (attackCooldownTimer >= attackCooldownDuration){
                        if (anim != null) anim.Play(attackAnimName);
                        weaponInHand.Attack();
                        attackCooldownTimer = 0f;
                    }
                } else {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                    if (anim != null) anim.Play(movementAnimName);
                }
            }
        } else {
            gameObject.SetActive(false);
        }
    }
}
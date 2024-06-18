using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Transform target;
    public bool canCauseKnockback = true;
    public Weapon weaponInHand;
    public float distanceStartAttacking;

    void Start(){
        base.Start();
        target = GameManager.Instance.player.transform;
    }

    void Update(){
        if (!GameManager.Instance.isGameOver){
            if (target != null && !GameManager.Instance.inTransition){
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, target.position) <= distanceStartAttacking){
                    weaponInHand.Attack();
                }
            }
        } else {
            gameObject.SetActive(false);
        }
    }
}
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
                Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                GetComponent<Rigidbody2D>().MovePosition(pos);
                if (Vector3.Distance(transform.position, target.position) <= distanceStartAttacking){
                    weaponInHand.Attack();
                }
            }
        } else {
            gameObject.SetActive(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float damage;
    public Transform target;
    public bool canCauseKnockback = true;

    void Start(){
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update(){
        if (!GameManager.Instance.isGameOver){
            if (target != null && !GameManager.Instance.inTransition){
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        } else {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<Player>().TakeDamage(damage, transform.position, canCauseKnockback);
        }
    }
}
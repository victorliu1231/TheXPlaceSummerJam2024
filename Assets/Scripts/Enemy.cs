using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float damage;
    public Transform target;

    void Start(){
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update(){
        if (target != null){
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Player"){
            other.GetComponent<Player>().TakeDamage(damage, transform.position);
        }
    }
}
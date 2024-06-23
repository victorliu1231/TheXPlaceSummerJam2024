using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectible : MonoBehaviour{
    public GameObject weaponPrefab;
    public new Collider2D collider2D;

    void Awake(){
        collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;
        Invoke("EnableCollider", 1f);
    }

    void EnableCollider(){
        collider2D.enabled = true;
    }
}
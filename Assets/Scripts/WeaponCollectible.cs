using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectible : MonoBehaviour{
    public GameObject weaponPrefab;
    private Collider2D collider;

    void Start(){
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        Invoke("EnableCollider", 1f);
    }

    void EnableCollider(){
        collider.enabled = true;
    }
}
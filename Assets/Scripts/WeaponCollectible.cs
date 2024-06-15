using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectible : MonoBehaviour{
    public Weapon weaponPrefab;
    public bool isCollectible = true;

    void OnTriggerExit2D(Collider2D collider){
        if (collider.gameObject.tag == "Player"){
            isCollectible = true;
        }
    }
}
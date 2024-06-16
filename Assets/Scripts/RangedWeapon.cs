using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Ranged Stats")]
    [Tooltip("The prefab that will damage enemies")]
    public GameObject prefab;

    void Start(){
        base.Start();
    }

    public override void Attack(){
        base.Attack();
        if (weaponType == WeaponType.Ranged) RangedAttack();
    }

    void RangedAttack(){
        Instantiate(prefab, transform.position, transform.rotation);
    }
}

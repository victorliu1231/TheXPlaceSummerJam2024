using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Ranged Stats")]
    public GameObject bulletPrefab;

    void Start(){
        base.Start();
    }

    public override void Attack(){
        base.Attack();
        if (weaponType == WeaponType.Ranged) RangedAttack();
    }

    void RangedAttack(){
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}

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

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0)){
            if (weaponType == WeaponType.Ranged) RangedAttack();
        }
    }

    void RangedAttack(){
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Ranged Stats")]
    [Tooltip("The prefab that will damage enemies")]
    public GameObject prefab;
    [Tooltip("Fire particles that will be played when attacking")]
    public ParticleSystem fireParticles;

    void Start(){
        base.Start();
    }

    public override void Attack(){
        base.Attack();
        if (weaponType == WeaponType.Ranged) {
            StartCoroutine(RangedAttack());
        }
    }

    IEnumerator RangedAttack(){
        Quaternion freezeWeaponRot = transform.rotation;
        yield return new WaitForSeconds(timeDelayBetweenPlayerPosStorageAndAttack);
        if (fireParticles != null) fireParticles.Play();
        GameObject projectile = Instantiate(prefab, transform.position, freezeWeaponRot, GameManager.Instance.projectilesParent);
        projectile.GetComponent<SpriteRenderer>().flipX = transform.localEulerAngles.z < -90 || transform.localEulerAngles.z > 90;
    }
}

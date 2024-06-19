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
            if (gameObject.tag == "Bow") AudioManager.GetSFX("ArrowFire")?.Play();
            else if (gameObject.tag == "Gun") AudioManager.GetSFX("GunFire")?.Play();
            else if (gameObject.tag == "Laser") AudioManager.GetSFX("LaserBeam")?.Play();
            else if (gameObject.tag == "FireballWeapon") AudioManager.GetSFX("SpellCast")?.Play();
            StartCoroutine(RangedAttack());
        }
    }

    IEnumerator RangedAttack(){
        Quaternion freezeWeaponRot = transform.rotation;
        if (!isInputControlled) yield return new WaitForSeconds(timeDelayBetweenPlayerPosStorageAndAttack);
        if (fireParticles != null) fireParticles.Play();
        if (gameObject.tag == "FireballWeapon") AudioManager.GetSFX("Fireball")?.Play();
        GameObject projectile = Instantiate(prefab, transform.position, freezeWeaponRot, GameManager.Instance.projectilesParent);
        if (GetComponent<TimeSlowdown>() != null) {
            projectile.GetComponent<Projectile>().isGhost = base.isGhost;
            projectile.GetComponent<TimeSlowdown>().ChangeStage(GetComponent<TimeSlowdown>().stage);
        }
        projectile.GetComponent<SpriteRenderer>().flipX = transform.localEulerAngles.z < -90 || transform.localEulerAngles.z > 90;
    }
}

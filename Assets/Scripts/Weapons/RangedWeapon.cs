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
    [Tooltip("The offset from the ranged weapon that projectiles will spawn at, multiplied by the projectile's scale.")]
    public float spawnOffset;
    public bool limitMaxActiveProjectiles = false;
    public int maxActiveProjectiles;
    public int numActiveProjectiles;
    public List<GameObject> activeProjectiles;

    new void Start(){
        base.Start();
        activeProjectiles = new List<GameObject>();
    }

    public override void Attack(){
        base.Attack();
        if (weaponType == WeaponType.Ranged) {
            if (gameObject.tag == "Bow") AudioManager.GetSFX("ArrowFire")?.Play();
            else if (gameObject.tag == "Gun") AudioManager.GetSFX("GunFire")?.Play();
            else if (gameObject.tag == "Laser") AudioManager.GetSFX("LaserBeam")?.Play();
            else if (gameObject.tag == "FireballWeapon") AudioManager.GetSFX("SpellCast")?.Play();
            if (limitMaxActiveProjectiles){ 
                if (numActiveProjectiles < maxActiveProjectiles) {
                    StartCoroutine(RangedAttack());
                } else {
                    Destroy(activeProjectiles[0]);
                    activeProjectiles.RemoveAt(0);
                    StartCoroutine(RangedAttack());
                }
            } else {
                StartCoroutine(RangedAttack());
            }
            numActiveProjectiles++;
        }
    }

    IEnumerator RangedAttack(){
        Quaternion freezeWeaponRot = transform.rotation;
        if (!isInputControlled) yield return new WaitForSeconds(timeDelayBetweenPlayerPosStorageAndAttack);
        if (fireParticles != null) fireParticles.Play();
        if (gameObject.tag == "FireballWeapon") AudioManager.GetSFX("Fireball")?.Play();
        Vector3 spawnPosition = transform.position;

        GameObject projectile = Instantiate(prefab, spawnPosition, freezeWeaponRot, GameManager.Instance.projectilesParent);
        if (GetComponent<TimeSlowdown>() != null) {
            if (GetComponent<TimeSlowdown>().stage < GameManager.Instance.stage) projectile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            projectile.GetComponent<Projectile>().isGhost = base.isGhost;
            projectile.GetComponent<TimeSlowdown>().ChangeStage(GetComponent<TimeSlowdown>().stage, transform.parent.tag == "Enemy");
        }
        activeProjectiles.Add(projectile);
    }
}

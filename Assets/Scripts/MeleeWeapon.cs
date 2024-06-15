using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Stats")]
    public float meleeDamage;
    public float meleeAnimationTime;
    public ParticleSystem hitParticles;
    private Collider2D collider;

    void Start(){
        base.Start();
        collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
    }

    public override void Attack(){
        base.Attack();
        if (weaponType == WeaponType.Melee) StartCoroutine(MeleeAttack());
    }

    IEnumerator MeleeAttack(){
        collider.enabled = true;
        yield return new WaitForSeconds(meleeAnimationTime);
        collider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Enemy" || other.tag == "Wall"){
            if (other.tag == "Enemy") other.GetComponent<Enemy>().TakeDamage(meleeDamage, transform.position);
            if (hitParticles != null) hitParticles.Play();
        }
    }
}

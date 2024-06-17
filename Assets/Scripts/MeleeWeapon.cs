using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Stats")]
    public float meleeDamage;
    public float meleeAnimationTime;
    public ParticleSystem hitParticles;
    public bool canCauseKnockback = true;
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
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall"){
            if (other.gameObject.tag == "Enemy") other.gameObject.GetComponent<Enemy>().TakeDamage(meleeDamage, transform.position, canCauseKnockback);
            Invoke("PlayParticles", meleeAnimationTime);
        }
    }

    void PlayParticles(){
        if (hitParticles != null) hitParticles.Play();
    }
}

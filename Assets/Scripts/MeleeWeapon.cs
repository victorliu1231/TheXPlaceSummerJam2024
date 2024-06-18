using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Stats")]
    public float meleeDamage;
    public float secAbleToHit;
    public ParticleSystem hitParticles;
    public bool canCauseKnockback = true;
    public float knockbackForce;
    public string targetTag;
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
        yield return new WaitForSeconds(secAbleToHit);
        collider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == targetTag || other.gameObject.tag == "Wall"){
            if (other.gameObject.tag == targetTag) other.gameObject.GetComponent<Entity>().TakeDamage(meleeDamage, transform.position, canCauseKnockback, knockbackForce);
            Invoke("PlayParticles", secAbleToHit);
        }
    }

    void PlayParticles(){
        if (hitParticles != null) hitParticles.Play();
    }
}

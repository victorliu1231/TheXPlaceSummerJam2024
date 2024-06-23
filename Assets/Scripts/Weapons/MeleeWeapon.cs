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
    private new Collider2D collider;
    public enum ParentType { Player, Enemy};
    public ParentType parentType;

    new void Start(){
        base.Start();
        collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
        //if (GetComponent<TimeSlowdown>() != null && parentType == ParentType.Player){
        //    meleeDamage = meleeDamage * 1f / GetComponent<TimeSlowdown>().stage;
        //}
    }

    public override void Attack(){
        base.Attack();
        if (weaponType == WeaponType.Melee) {
            if (gameObject.tag == "SwingableMeleeObject") AudioManager.GetSFX("MeleeSwing")?.Play();
            StartCoroutine(MeleeAttack());
        }
    }

    IEnumerator MeleeAttack(){
        if (collider != null) collider.enabled = true;
        yield return new WaitForSeconds(secAbleToHit);
        if (collider != null) collider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == targetTag || other.gameObject.tag == "Wall"){
            if (other.gameObject.tag == targetTag) other.gameObject.GetComponent<Entity>().TakeDamage(meleeDamage, transform.position, canCauseKnockback, knockbackForce, gameObject);
            Invoke("PlayParticles", secAbleToHit * Util.GetStage(GetComponent<TimeSlowdown>()));
            if (transform.parent.parent.tag == "Player") Debug.Log("Hit " + other.gameObject.name + " with " + meleeDamage + " damage!");
        }
    }

    void PlayParticles(){
        if (hitParticles != null) hitParticles.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    public float supportCooldownDuration;
    public float speedUpAlliesMultiplier;
    [Tooltip("Duration in seconds that allies are sped up. Should be less than supportCooldownDuration.")]
    public float durationAlliesSpedUp;
    public float radiusToSupport;
    private float supportCooldownTimer = 0f;
    private bool isSupportMoveEnabled = false;
    public GameObject supportAnim;
    public string supportAnimName;
        
    void Start(){
        base.Start();

        Invoke("EnableSupportMove", 2.5f * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>()));
        supportAnim.SetActive(false);
    }

    public override void Update(){
        base.Update();
        if (!GameManager.Instance.isGameOver){
            if (target != null && !GameManager.Instance.inTransition && isSupportMoveEnabled){
                supportCooldownTimer += Time.deltaTime * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>());
                if (supportCooldownTimer >= supportCooldownDuration){
                    SpeedUpAllies();
                    supportCooldownTimer = 0f;
                }
            }
        }
    }

    public void EnableSupportMove(){
        isSupportMoveEnabled = true;
    }

    public void SpeedUpAllies(){
        AudioManager.GetSFX("SpellCast")?.Play();
        if (anim != null) anim.Play(supportAnimName);
        supportAnim.SetActive(true);
        // Detect all enemies in radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radiusToSupport);
        foreach (Collider2D collider in colliders){
            if (collider.tag == "Enemy"){
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != this){
                    StartCoroutine(SpeedUpAlliesCo(enemy));
                }
            }
        }
    }

    public IEnumerator SpeedUpAlliesCo(Enemy enemy){
        if (!enemy.isAttackCooldownReduced){
            enemy.isAttackCooldownReduced = true;
            enemy.attackCooldownSprite.enabled = true;
            float originalAttackCooldownDuration = enemy.attackCooldownDuration;
            enemy.attackCooldownDuration *= speedUpAlliesMultiplier;
            yield return new WaitForSeconds(durationAlliesSpedUp * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>()));
            enemy.attackCooldownDuration = originalAttackCooldownDuration;
            enemy.isAttackCooldownReduced = false;
            enemy.attackCooldownSprite.enabled = false;
        }
    }
}
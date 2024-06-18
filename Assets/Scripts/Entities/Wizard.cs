using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    public float supportCooldownDuration;
    public float speedUpAlliesMultiplier;
    private float supportCooldownTimer = 0f;
    private bool isSupportMoveEnabled = false;
    public GameObject supportAnim;
    public string supportAnimName;

    void Start(){
        base.Start();
        Invoke("EnableSupportMove", 2.5f);
        supportAnim.SetActive(false);
    }

    public override void Update(){
        base.Update();
        if (!GameManager.Instance.isGameOver){
            if (target != null && !GameManager.Instance.inTransition && isSupportMoveEnabled){
                supportCooldownTimer += Time.deltaTime;
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
        if (anim != null) anim.Play(supportAnimName);
        supportAnim.SetActive(true);
        foreach (Transform child in GameManager.Instance.enemiesParent){
            Enemy enemy = child.gameObject.GetComponent<Enemy>();
            if (enemy != this){
                enemy.attackCooldownDuration *= speedUpAlliesMultiplier;
            }
        }
    }
}
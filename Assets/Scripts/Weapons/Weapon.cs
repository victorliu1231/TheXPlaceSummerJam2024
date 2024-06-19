using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType {Melee, Ranged};
    public enum FaceDirection { Up, Down, Left, Right};
    [Header("General Weapon Stats")]
    public WeaponType weaponType;
    public FaceDirection faceDirection;
    public bool isInputControlled;
    public bool isGhost = false;
    public Vector2 ghostMousePos;
    private Animator anim;
    private float cooldownTimer = 0f;
    private Transform player;
    public Transform target;
    [Header("If Input Controlled")]
    public float cooldownDuration;
    public GameObject weaponCollectible;
    [Header("If Enemy Controlled")]
    [Tooltip("This time delay is crucial for letting the player dodge instantaneous attacks like a laser beam")]
    public float timeDelayBetweenPlayerPosStorageAndAttack;

    public void Start(){
        anim = GetComponent<Animator>();
        player = GameManager.Instance.player.transform;
        if (!isGhost) GetComponent<TimeSlowdown>()?.ChangeStage(GameManager.Instance.stage);
    }

    public void BindTarget(Transform target){
        this.target = target;
    }

    // Update is called once per frame
    public void Update()
    {
        if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            isGhost = true;
        }
        else
        {
            isGhost = false;
        }
        if (!GameManager.Instance.inTransition && !GameManager.Instance.isGameOver){
            if (isInputControlled || isGhost){
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                PlayerGhost playerGhost = transform.parent.parent.GetComponent<PlayerGhost>();
                Player playerScript = player.GetComponent<Player>();
                if (isGhost)
                {
                    mousePos = ghostMousePos;
                }
                Vector3 lookDir = (mousePos - transform.position) * player.localScale.x;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                if (faceDirection == FaceDirection.Up){
                    if ((-45f < angle && angle <= 45f-22.5f) || (-405f < angle && angle <= -315f-22.5f)) {
                        faceDirection = FaceDirection.Right;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.rightWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorRightBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.rightWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorRightBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = false;
                    }
                    else if ((-135f < angle && angle <= -45f) || (-315f < angle && angle <= -225f)) {
                        faceDirection = FaceDirection.Down;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.downWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorDownBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.downWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorDownBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = false;
                    }
                    else if ((-225f + 22.5f < angle && angle <= -135f) || (135f + 22.5f < angle && angle <= 225f)) {
                        faceDirection = FaceDirection.Left;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.leftWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorLeftBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.leftWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorLeftBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = true;
                    }
                }
                if (faceDirection == FaceDirection.Right){
                    if ((45f +22.5f < angle && angle <= 135f) || (-315f+22.5f < angle && angle <= -225f)) {
                        faceDirection = FaceDirection.Up;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.upWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorUpBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.upWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorUpBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = false;
                    }
                    else if ((-135f < angle && angle <= -45f - 22.5f) || (-315f < angle && angle <= -225f - 22.5f)) {
                        faceDirection = FaceDirection.Down;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.downWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorDownBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.downWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorDownBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = false;
                    }
                    else if ((-225f < angle && angle <= -135f) || (135f < angle && angle <= 225f)) {
                        faceDirection = FaceDirection.Left;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.leftWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorLeftBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.leftWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorLeftBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = true;
                    }
                }
                if (faceDirection == FaceDirection.Down){
                    if ((45f < angle && angle <= 135f) || (-315f < angle && angle <= -225f)) {
                        faceDirection = FaceDirection.Up;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.upWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorUpBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.upWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorUpBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = false;
                    }
                    else if ((-45f +22.5f< angle && angle <= 45f) || (-405f +22.5f< angle && angle <= -315f)) {
                        faceDirection = FaceDirection.Right;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.rightWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorRightBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.rightWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorRightBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = false;
                    }
                    else if ((-225f < angle && angle <= -135f - 22.5f) || (135f < angle && angle <= 225f - 22.5f)) {
                        faceDirection = FaceDirection.Left;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.leftWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorLeftBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.leftWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorLeftBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = true;
                    }
                }
                if (faceDirection == FaceDirection.Left){
                    if ((45f < angle && angle <= 135f - 22.5f) || (-315f < angle && angle <= -225f - 22.5f)) {
                        faceDirection = FaceDirection.Up;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.upWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorUpBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.upWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorUpBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = false;
                    }
                    else if ((-45f < angle && angle <= 45f) || (-405f < angle && angle <= -315f)) {
                        faceDirection = FaceDirection.Right;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.rightWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorRightBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.rightWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorRightBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = false;
                    }
                    else if ((-135f + 22.5f < angle && angle <= -45f) || (-315f + 22.5f < angle && angle <= -225f)) {
                        faceDirection = FaceDirection.Down;
                        if (isGhost)
                        {
                            transform.SetParent(playerGhost.downWeaponBinding, false);
                            playerGhost.stageIndicator.SetParent(playerGhost.stageIndicatorDownBinding, false);
                        }
                        else
                        {
                            transform.SetParent(playerScript.downWeaponBinding, false);
                            playerScript.stageIndicator.SetParent(playerScript.stageIndicatorDownBinding, false);
                        }
                        GetComponent<SpriteRenderer>().flipY = false;
                    }
                }
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }else {
                Vector3 lookDir = target.position - transform.position;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }   

            cooldownTimer += Time.deltaTime;
        }
    }

    public virtual void TryAttack()
    {
        if (cooldownTimer >= cooldownDuration * Util.GetStage(GetComponent<TimeSlowdown>()))
        {
            Attack();
            cooldownTimer = 0f;
        }
    }
    public virtual void Attack(){
        if (anim != null){
            anim.SetTrigger("Active");
        }
    }
}

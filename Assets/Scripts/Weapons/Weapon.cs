using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType {Melee, Ranged};
    [Header("General Weapon Stats")]
    public WeaponType weaponType;
    public bool isInputControlled;
    private Animator anim;
    private float cooldownTimer = 0f;
    private Transform player;
    [Header("If Input Controlled")]
    public float cooldownDuration;
    public float minRotateAngle;
    public float maxRotateAngle;
    public GameObject weaponCollectible;

    public void Start(){
        anim = GetComponent<Animator>();
        player = GameManager.Instance.player.transform;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!GameManager.Instance.inTransition && !GameManager.Instance.isGameOver){
            if (isInputControlled){
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 lookDir = (mousePos - transform.position) * player.localScale.x;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                float rePositionAngle;
                Player playerComponent = player.GetComponent<Player>();
                if (playerComponent.faceDirection == Player.FaceDirection.Left || playerComponent.faceDirection == Player.FaceDirection.Right) rePositionAngle = 0f;
                else if (playerComponent.faceDirection == Player.FaceDirection.Up) rePositionAngle = 90f;
                else rePositionAngle = -90f;

                if (GameManager.Instance.player.GetComponent<Player>().faceDirection == Player.FaceDirection.Up) angle = Mathf.Abs(angle);
                if (GameManager.Instance.player.GetComponent<Player>().faceDirection == Player.FaceDirection.Down) angle = Mathf.Abs(angle) * -1;

                if (angle < minRotateAngle + rePositionAngle) angle = minRotateAngle + rePositionAngle;
                if (angle > maxRotateAngle + rePositionAngle) angle = maxRotateAngle + rePositionAngle;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }else {
                Vector3 lookDir = player.position - transform.position;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }   

            cooldownTimer += Time.deltaTime;
        }
    }

    public virtual void TryAttack()
    {
        if (cooldownTimer >= cooldownDuration)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("General Weapon Stats")]
    public float cooldownDuration;
    public enum WeaponType {Melee, Ranged};
    public WeaponType weaponType;
    public float minRotateAngle;
    public float maxRotateAngle;
    public GameObject weaponCollectible;
    private Animator anim;
    private float cooldownTimer = 0f;
    private Transform player;

    public void Start(){
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!GameManager.Instance.inTransition){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lookDir = (mousePos - transform.position) * player.localScale.x;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            float rePositionAngle;
            Player playerComponent = player.GetComponent<Player>();
            if (playerComponent.faceDirection == Player.FaceDirection.Left || playerComponent.faceDirection == Player.FaceDirection.Right) rePositionAngle = 0f;
            else if (playerComponent.faceDirection == Player.FaceDirection.Up) rePositionAngle = 90f;
            else rePositionAngle = -90f;
            if (angle < minRotateAngle + rePositionAngle) angle = minRotateAngle + rePositionAngle;
            if (angle > maxRotateAngle + rePositionAngle) angle = maxRotateAngle + rePositionAngle;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            cooldownTimer += Time.deltaTime;

            if (Input.GetMouseButtonDown(0)){
                if (cooldownTimer >= cooldownDuration){
                    Attack();
                    cooldownTimer = 0f;
                }
            }
        }
    }

    public virtual void Attack(){
        if (anim != null){
            anim.SetTrigger("Active");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("General Weapon Stats")]
    public float cooldownDuration;
    public enum WeaponType {Melee, Ranged};
    public WeaponType weaponType;
    private Animator anim;
    private float cooldownTimer = 0f;

    public void Start(){
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        cooldownTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0)){
            if (cooldownTimer >= cooldownDuration){
                Attack();
                cooldownTimer = 0f;
            }
        }
    }

    public virtual void Attack(){
        if (anim != null){
            anim.SetTrigger("Active");
        }
    }
}

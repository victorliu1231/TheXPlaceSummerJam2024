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
    private Collider2D collider;

    public void Start(){
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    // Update is called once per frame
    public void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButtonDown(0)){
            if (anim != null){
                anim.enabled = false;
                anim.enabled = true;
            }
        }
    }
}

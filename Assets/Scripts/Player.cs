using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private Animator anim;
    public Weapon weaponInHand;
    public Transform sideWeaponBinding;
    public Transform upWeaponBinding;
    public Transform downWeaponBinding;


    void Start(){
        base.Start();
        anim = GetComponent<Animator>();
        weaponInHand = GetComponentInChildren<Weapon>();
        if (weaponInHand != null) weaponInHand.transform.SetParent(sideWeaponBinding);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.inTransition){
            float horizontal = Input.GetAxis("Horizontal") * speed;
            float vertical = Input.GetAxis("Vertical") * speed;
            if (horizontal != 0){
                anim.Play("Player_Run_Right");
                Vector3 newScale = transform.localScale;
                newScale.x = horizontal < 0 ? -1f : 1f;
                transform.localScale = newScale;
                weaponInHand.transform.SetParent(sideWeaponBinding);
            }
            if (vertical > 0){
                anim.Play("Player_Run_Up");
                weaponInHand.transform.SetParent(upWeaponBinding);
            }
            else if (vertical < 0){
                anim.Play("Player_Run_Down");
                weaponInHand.transform.SetParent(downWeaponBinding);
            }
            transform.position = new Vector3(transform.position.x + horizontal, transform.position.y + vertical, transform.position.z);
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.tag == "Wall"){
            transform.rotation = Quaternion.identity;
        }
    }
}

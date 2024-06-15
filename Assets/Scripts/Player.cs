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
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;
        if (anim != null){
            if (horizontal != 0){
                anim.Play("Player_Run_Right");
                Vector3 newScale = transform.localScale;
                newScale.x = horizontal < 0 ? -1f : 1f;
                transform.localScale = newScale;
            }
            else if (vertical > 0){
                anim.Play("Player_Run_Up");
                weaponInHand.transform.SetParent(upWeaponBinding);
            }
            else if (vertical < 0){
                anim.Play("Player_Run_Down");
                weaponInHand.transform.SetParent(downWeaponBinding);
            }
            else {
                anim.Play("Player_Idle");
                weaponInHand.transform.SetParent(sideWeaponBinding);
            }
        }
        transform.position = new Vector3(transform.position.x + horizontal, transform.position.y + vertical, transform.position.z);
    }

    void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.tag == "Wall"){
            transform.rotation = Quaternion.identity;
        }
    }
}

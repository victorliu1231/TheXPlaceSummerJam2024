using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start(){
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;
        if (anim != null){
            if (horizontal != 0){
                anim.Play("Player_Run_Right");
                if (spriteRenderer != null) spriteRenderer.flipX = horizontal < 0;
            }
            else if (vertical > 0){
                anim.Play("Player_Run_Up");
            }
            else if (vertical < 0){
                anim.Play("Player_Run_Down");
            }
            else {
                anim.Play("Player_Idle");
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

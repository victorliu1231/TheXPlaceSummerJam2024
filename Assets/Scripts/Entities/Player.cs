using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [HideInInspector]public Animator anim;
    public Weapon weaponInHand;
    public Transform rightWeaponBinding;
    public Transform leftWeaponBinding;
    public Transform upWeaponBinding;
    public Transform downWeaponBinding;
    public Transform stageIndicatorRightBinding;
    public Transform stageIndicatorLeftBinding;
    public Transform stageIndicatorUpBinding;
    public Transform stageIndicatorDownBinding;
    public Transform stageIndicator;
    public enum FaceDirection{ Up, Down, Left, Right};
    public FaceDirection faceDirection;

    void Start(){
        base.Start();
        anim = GetComponent<Animator>();
        weaponInHand = GetComponentInChildren<Weapon>();
        if (weaponInHand != null) weaponInHand.transform.SetParent(rightWeaponBinding);
        stageIndicator.SetParent(stageIndicatorRightBinding);
        faceDirection = FaceDirection.Right;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.inTransition && !GameManager.Instance.isGameOver){
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (weaponInHand?.faceDirection == Weapon.FaceDirection.Right){
                if (horizontal > 0){
                    anim.Play("Player_Run_Right");
                } else {
                    anim.Play("Player_Idle_Right");
                }
            } else if (weaponInHand?.faceDirection == Weapon.FaceDirection.Left){
                if (horizontal < 0){
                    anim.Play("Player_Run_Left");
                } else {
                    anim.Play("Player_Idle_Left");
                }
            }
            else if (weaponInHand?.faceDirection == Weapon.FaceDirection.Up){
                if (vertical > 0) anim.Play("Player_Run_Up");
                else anim.Play("Player_Up_Idle");
            }
            else if (weaponInHand?.faceDirection == Weapon.FaceDirection.Down){
                if (vertical < 0) anim.Play("Player_Run_Down");
                else anim.Play("Player_Down_Idle");
            }
            Vector3 targetPos = new Vector3(transform.position.x + horizontal, transform.position.y + vertical, transform.position.z);
            Vector3 pos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(pos);

            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<PlayerRecorder>()?.queuedActions.Add(Snapshot.SnapAction.Fire);
                weaponInHand?.TryAttack();
            }

            if (Input.GetKeyDown(KeyCode.Space) && weaponInHand != null){
                Transform bindingParent = weaponInHand.transform.parent;
                GameObject droppedWeaponCollectible = Instantiate(weaponInHand.weaponCollectible, transform.position, Quaternion.identity);
                foreach (Transform child in bindingParent){
                    Destroy(child.gameObject);
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy"){
            transform.rotation = Quaternion.identity;
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Collectible"){
            WeaponCollectible weaponCollectible = collider.gameObject.GetComponent<WeaponCollectible>();
            if (weaponCollectible != null && weaponInHand == null){
                GameObject newWeapon = Instantiate(weaponCollectible.weaponPrefab, transform.position, Quaternion.identity, rightWeaponBinding);
                weaponInHand = newWeapon.GetComponent<Weapon>();
                Destroy(collider.gameObject);
            }
        }
    }

    public override void Die(){
        StopAllCoroutines();
        healthbar.gameObject.SetActive(false);
        StartCoroutine(GameManager.Instance.GameOver(false));
    }
}

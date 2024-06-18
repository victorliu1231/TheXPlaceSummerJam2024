using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [HideInInspector]public Animator anim;
    public Weapon weaponInHand;
    public Transform sideWeaponBinding;
    public Transform upWeaponBinding;
    public Transform downWeaponBinding;
    public enum FaceDirection{ Up, Down, Left, Right};
    public FaceDirection faceDirection;

    void Start(){
        base.Start();
        anim = GetComponent<Animator>();
        weaponInHand = GetComponentInChildren<Weapon>();
        if (weaponInHand != null) weaponInHand.transform.SetParent(sideWeaponBinding);
        faceDirection = FaceDirection.Right;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.inTransition && !GameManager.Instance.isGameOver){
            float horizontal = Input.GetAxis("Horizontal") * speed;
            float vertical = Input.GetAxis("Vertical") * speed;
            if (horizontal != 0){
                anim.Play("Player_Run_Right");
                Vector3 newScale = transform.localScale;
                newScale.x = horizontal < 0 ? Mathf.Abs(transform.localScale.x)*-1f : Mathf.Abs(transform.localScale.x)*1f;
                transform.localScale = newScale;
                faceDirection = horizontal < 0 ? FaceDirection.Left : FaceDirection.Right;
                weaponInHand?.transform.SetParent(sideWeaponBinding);
            }
            else if (vertical > 0){
                anim.Play("Player_Run_Up");
                faceDirection = FaceDirection.Up;
                weaponInHand?.transform.SetParent(upWeaponBinding);
            }
            else if (vertical < 0){
                anim.Play("Player_Run_Down");
                faceDirection = FaceDirection.Down;
                weaponInHand?.transform.SetParent(downWeaponBinding);
            }
            transform.position = new Vector3(transform.position.x + horizontal, transform.position.y + vertical, transform.position.z);

            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<PlayerRecorder>()?.queuedActions.Add(Snapshot.SnapAction.Fire);
                weaponInHand?.TryAttack();
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
            if (weaponCollectible != null){
                if (weaponInHand is not null)
                {
                    Transform bindingParent = weaponInHand.transform.parent;
                    GameObject droppedWeaponCollectible = Instantiate(weaponInHand.weaponCollectible, transform.position, Quaternion.identity);
                    foreach (Transform child in bindingParent){
                        Destroy(child.gameObject);
                    }
                }

                GameObject newWeapon = Instantiate(weaponCollectible.weaponPrefab, transform.position, Quaternion.identity);
                weaponInHand = newWeapon.GetComponent<Weapon>();
                Destroy(collider.gameObject);
            }
        }
    }

    public override void Die(){
        StartCoroutine(GameManager.Instance.GameOver(false));
    }
}
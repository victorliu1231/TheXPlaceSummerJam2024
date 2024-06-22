using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : Entity
{
    [HideInInspector] public Animator anim;
    public Weapon weaponInHand;
    public Transform leftWeaponBinding;
    public Transform rightWeaponBinding;
    public Transform upWeaponBinding;
    public Transform downWeaponBinding;
    public Transform stageIndicatorRightBinding;
    public Transform stageIndicatorLeftBinding;
    public Transform stageIndicatorUpBinding;
    public Transform stageIndicatorDownBinding;
    public Transform stageIndicator;
    public enum FaceDirection { Up, Down, Left, Right };
    public FaceDirection faceDirection;

    public Queue<Snapshot> snapshots;
    public float firstTime;
    public Rigidbody2D rb;

    private Vector2 lastSnapPos;
    private bool hasSnapped = false;

    void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        weaponInHand = GetComponentInChildren<Weapon>();
        if (weaponInHand != null) weaponInHand.transform.SetParent(rightWeaponBinding);
        stageIndicator.SetParent(stageIndicatorRightBinding);
        faceDirection = FaceDirection.Right;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        firstTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        int stage = GameManager.Instance.stage - (GetComponent<TimeSlowdown>()?.stage ?? 1) + 1;

        if (!GameManager.Instance.inTransition && !GameManager.Instance.isGameOver)
        {
            if (weaponInHand?.faceDirection == Weapon.FaceDirection.Right)
            {
                anim.Play("Player_Idle_Right");
            }
            else if (weaponInHand?.faceDirection == Weapon.FaceDirection.Left)
            {
                anim.Play("Player_Idle_Left");
            }
            else if (weaponInHand?.faceDirection == Weapon.FaceDirection.Up)
            {
                anim.Play("Player_Up_Idle");
            }
            else if (weaponInHand?.faceDirection == Weapon.FaceDirection.Down)
            {
                anim.Play("Player_Down_Idle");
            }

            if (snapshots.Count > 0)
            {
                Snapshot nextSnap = snapshots.Peek();

                if (nextSnap is not null && nextSnap.timeSinceFirstSnapshot * stage <= Time.time - firstTime)
                {
                    nextSnap = snapshots.Dequeue();

                    if (weaponInHand) weaponInHand.ghostMousePos = nextSnap.mousePos;

                    if (!rb) { rb = GetComponent<Rigidbody2D>(); }

                    nextSnap.rbState.SetRigidbody(rb);
                    if (!hasSnapped)
                    {
                        hasSnapped = true;
                        rb.position = nextSnap.pos;
                    }
                    else
                    {
                        rb.position += nextSnap.pos - lastSnapPos;
                    }
                    lastSnapPos = nextSnap.pos;
                    foreach (Snapshot.SnapAction action in nextSnap.actions)
                    {
                        switch (action)
                        {
                            case Snapshot.SnapAction.Fire:
                                weaponInHand?.TryAttack();
                                break;
                            case Snapshot.SnapAction.DropWeapon:
                                DropWeapon();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else if (snapshots.Count == 0)
            {
                Die();
            }
        }
    }

    void DropWeapon(){
        if (weaponInHand != null) {
            Transform bindingParent = weaponInHand.transform.parent;
            GameObject droppedWeaponCollectible = Instantiate(weaponInHand.weaponCollectible, transform.position, Quaternion.identity, GameManager.Instance.collectiblesParent);
            droppedWeaponCollectible.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            foreach (Transform child in bindingParent){
                Destroy(child.gameObject);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy")
        {
            transform.rotation = Quaternion.identity;
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Collectible"){
            WeaponCollectible weaponCollectible = collider.gameObject.GetComponent<WeaponCollectible>();
            if (weaponCollectible != null && weaponInHand == null){
                GameObject newWeapon = Instantiate(weaponCollectible.weaponPrefab, transform.position, Quaternion.identity, rightWeaponBinding);
                newWeapon.GetComponent<TimeSlowdown>().ChangeStage(GetComponent<TimeSlowdown>().stage, false);
                newWeapon.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                weaponInHand = newWeapon.GetComponent<Weapon>();
                weaponInHand.isInputControlled = false;
                weaponInHand.isGhost = true;
                Destroy(collider.gameObject);
            }
        }
    }

    public override void Die()
    {
        base.Die();
    }
}

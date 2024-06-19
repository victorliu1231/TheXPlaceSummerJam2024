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
        faceDirection = FaceDirection.Right;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        firstTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.inTransition && !GameManager.Instance.isGameOver)
        {
            if (snapshots.Count > 0)
            {
                Snapshot nextSnap = snapshots.Peek();

                if (nextSnap is not null && nextSnap.timeSinceFirstSnapshot <= Time.time - firstTime)
                {
                    nextSnap = snapshots.Dequeue();

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

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy")
        {
            transform.rotation = Quaternion.identity;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Collectible")
        {
            WeaponCollectible weaponCollectible = collider.gameObject.GetComponent<WeaponCollectible>();
            if (weaponCollectible != null)
            {
                if (weaponInHand is not null)
                {
                    Transform bindingParent = weaponInHand.transform.parent;
                    GameObject droppedWeaponCollectible = Instantiate(weaponInHand.weaponCollectible, transform.position, Quaternion.identity);
                    foreach (Transform child in bindingParent)
                    {
                        Destroy(child.gameObject);
                    }
                }

                GameObject newWeapon = Instantiate(weaponCollectible.weaponPrefab, rightWeaponBinding);
                weaponInHand = newWeapon.GetComponent<Weapon>();
                Destroy(collider.gameObject);
            }
        }
    }

    public override void Die()
    {
        base.Die();
    }
}

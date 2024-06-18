using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeball : Enemy
{
    public float teleportCooldownDuration;
    private float teleportCooldownTimer;
    public string beginTeleportAnimName;
    public string endTeleportAnimName;
    private Collider2D clockWalls;

    public void Start(){
        base.Start();
        clockWalls = GameObject.FindGameObjectWithTag("Wall").GetComponent<Collider2D>();
    }

    public override void Update(){
        if (!GameManager.Instance.isGameOver){
            if (target != null && !GameManager.Instance.inTransition){
                teleportCooldownDuration += Time.deltaTime;
                if (teleportCooldownDuration >= teleportCooldownDuration){
                    if (anim != null) anim.Play(beginTeleportAnimName);
                    // Link teleport anim to attack anim thru animator
                    StartCoroutine(EyeballAttack());
                }
            }
        } else {
            gameObject.SetActive(false);
        }
    }

    public IEnumerator EyeballAttack(){
        yield return new WaitForSeconds(1f);
        // Teleport to a position that is distanceStartAttacking from player and also not within the clock walls
        TeleportAwayFromPlayer();
        if (anim != null) anim.Play(endTeleportAnimName);
        yield return new WaitForSeconds(1f);
        if (anim != null) anim.Play(movementAnimName);
        yield return new WaitForSeconds(1.5f);
        if (anim != null) anim.Play(attackAnimName);
        weaponInHand.Attack();
        teleportCooldownDuration = 0f;
    }

    void TeleportAwayFromPlayer() {
        // Calculate the direction from the player to the eyeball
        Vector3 direction = (transform.position - GameManager.Instance.player.transform.position).normalized;

        // Calculate a position that is distanceStartAttacking units away from the player in the direction of the eyeball
        Vector3 newPosition = GameManager.Instance.player.transform.position + direction * distanceStartAttacking;

        // Check if the new position is within the clock walls
        if (clockWalls.bounds.Contains(newPosition)) {
            TeleportAwayFromPlayer();
            return;
        }

        // Set the position of the eyeball to the new position
        transform.position = newPosition;
    }
}
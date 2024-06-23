using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeball : Enemy
{
    public float teleportCooldownDuration;
    private float teleportCooldownTimer;
    public string beginTeleportAnimName;
    public string endTeleportAnimName;
    [Tooltip("Used only for ghosts.")]

    int seed;

    void Awake()
    {
        seed = (int)transform.position.x + (int)transform.position.y;
    }

    new void Start(){
        base.Start();
        teleportCooldownTimer = teleportCooldownDuration; // speeds up first attack
    }

    public override void Update(){
        if (!GameManager.Instance.isGameOver){
            if (target != null && !GameManager.Instance.inTransition){
                teleportCooldownTimer += Time.deltaTime * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>());
                if (teleportCooldownTimer >= teleportCooldownDuration){
                    teleportCooldownTimer = 0f;
                    StartCoroutine(EyeballAttack());
                }
            } else {
                StopCoroutine(EyeballAttack());
            }
        } else {
            gameObject.SetActive(false);
        }
    }

    public IEnumerator EyeballAttack(){
        if (anim != null) anim.Play(beginTeleportAnimName);
        yield return new WaitForSeconds(0.5f * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>()));
        AudioManager.GetSFX("Teleport")?.Play();
        yield return new WaitForSeconds(0.5f * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>()));
        TeleportAwayFromPlayer();
        if (anim != null) anim.Play(endTeleportAnimName);
        yield return new WaitForSeconds(1f * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>()));
        if (anim != null) anim.Play(movementAnimName);
        yield return new WaitForSeconds(1.5f * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>()));
        if (anim != null) anim.Play(attackAnimName);
        yield return new WaitForSeconds(0.75f * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>()));
        weaponInHand.Attack();
        yield return new WaitForSeconds(1f * Util.GetRecriprocalStage(GetComponent<TimeSlowdown>()));
        if (anim != null) anim.Play(movementAnimName);
    }

    void TeleportAwayFromPlayer(bool newSeed = true)
    {
        if (newSeed) Random.InitState(seed);
        Vector3 direction = Random.insideUnitCircle.normalized;
        // Calculate a position that is distanceStartAttacking units away from the player in the direction of the eyeball
        Vector3 newPosition = GameManager.Instance.player.transform.position + direction * distanceStartAttacking;

        // Check if the new position is outside radius of the clock walls
        if (Vector3.Distance(Vector3.zero, newPosition) > GameManager.Instance.clockRadius){
            TeleportAwayFromPlayer(false);
            return;
        }
        // Set the position of the eyeball to the new position
        transform.position = newPosition;
    }
}
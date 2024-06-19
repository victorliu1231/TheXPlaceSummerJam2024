using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : Enemy
{   
    // Chooses an enemy and spawns a wall in front of them, as long as that wall does not collide with the player or other enemies
    public float spawnWallDistance;
    public float spawnWallCooldownDuration;
    private float spawnWallCooldownTimer = 0f;
    public GameObject wallPrefab;
    [Tooltip("This is the last target the squid has chosen to spawn a wall in front of.")]
    public GameObject lastWallTarget;

    public override void Update(){
        base.Update();
        if (!GameManager.Instance.isGameOver){
            if (target != null && !GameManager.Instance.inTransition){
                spawnWallCooldownTimer += Time.deltaTime;
                if (spawnWallCooldownTimer >= spawnWallCooldownDuration){
                    SpawnWall();
                    spawnWallCooldownTimer = 0f;
                }
            }
        }
    }

    void SpawnWall(){
        //lastWallTarget = GameManager.Instance.enemiesParent.GetChild(0).gameObject;
        //Vector3 spawnPos = target.position + (target.position - transform.position).normalized * spawnWallDistance;
        //GameObject wall = Instantiate(wallPrefab, spawnPos, Quaternion.identity, GameManager.Instance.spawnedWallsParent);
    }
}
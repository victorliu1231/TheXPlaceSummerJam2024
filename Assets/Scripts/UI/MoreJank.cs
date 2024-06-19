using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreJank : MonoBehaviour
{
    // only useful for timing things within the tutorial. very temporary
    public float beginClockDelay;
    public float beginSpawnDelay;

    void Start(){
        StartCoroutine(BeginClock());
        StartCoroutine(BeginSpawn());
    }

    IEnumerator BeginClock(){
        yield return new WaitForSeconds(beginClockDelay);
        GameManager.Instance.isBeginTime = true;
    }

    IEnumerator BeginSpawn(){
        yield return new WaitForSeconds(beginSpawnDelay);
        GameManager.Instance.currentLevelTemplate = GameManager.Instance.allLevels[Random.Range(0, GameManager.Instance.allLevels.Count - 1)];
        GameManager.Instance.currentLevelInstance = Instantiate(GameManager.Instance.currentLevelTemplate, Vector2.zero, Quaternion.identity);
    }
}

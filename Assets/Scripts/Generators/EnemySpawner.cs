using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnTime;
    public GameObject spawnObject;

    void Start()
    {
        StartCoroutine(Spawn(spawnObject));
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    public IEnumerator Spawn(GameObject go)
    {
        yield return new WaitForSeconds(spawnTime * GameManager.Instance.stage);
        GameObject enemy = Instantiate(go, transform.position, Quaternion.identity);

        GameObject parent = GameObject.FindWithTag("EnemyParentTransform");
        enemy.transform.parent = parent.transform;
    }
}

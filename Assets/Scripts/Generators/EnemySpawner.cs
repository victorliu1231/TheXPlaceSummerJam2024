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
        if (GameManager.Instance.stage > 1)
        {
            enemy.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }

        enemy.transform.parent = GameManager.Instance.enemiesParent;
    }
}

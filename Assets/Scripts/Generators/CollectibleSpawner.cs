using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
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
        GameObject collectible = Instantiate(go, transform.position, Quaternion.identity);
        if (GameManager.Instance.stage > 1)
        {
            collectible.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }

        collectible.transform.parent = GameManager.Instance.collectiblesParent;
    }
}

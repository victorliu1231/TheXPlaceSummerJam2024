using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularEnemyGenerator : MonoBehaviour
{
    [Tooltip("List of enemy prefabs to spawn.")]
    public List<GameObject> enemyPrefabs;
    [Tooltip("Weights of spawn chances for each enemy prefab")]
    public List<float> enemySpawnWeights;
    private Transform enemyParent; // Parent object for all enemies.
    private float timeUntilSpawn; // Time until next enemy spawns.
    private float timeUntilStop = 0f; // Time until enemy spawning stops.
    [Tooltip("Minimum time until next enemy spawns")]
    public float minimumSpawnTime; // Minimum time until next enemy spawns
    [Tooltip("Maximum time until next enemy spawns")]
    public float maximumSpawnTime; // Maximum time until next enemy spawns
    [Tooltip("Time until enemy spawning stops")]
    public float stopSpawnTime; // Time until enemy spawning stops

    // Start is called before the first frame update
    void Start()
    {
        NormalizeSpawnWeights();
        enemyParent = GameObject.FindGameObjectWithTag("EnemyParentTransform").transform; // Find the object with name "Enemies" to serve as parent object for all enemies.
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isGameOver){
            timeUntilSpawn -= Time.deltaTime;
            timeUntilStop += Time.deltaTime;

            if (timeUntilSpawn <= 0 && timeUntilStop <= stopSpawnTime){
                SpawnEnemy();
                SetTimeUntilSpawn();
            }
        } else {
            gameObject.SetActive(false);
        }
    }

    void NormalizeSpawnWeights(){
        float sum = 0;
        foreach (float weight in enemySpawnWeights){
            sum += weight;
        }
        for (int i = 0; i < enemySpawnWeights.Count; i++){
            enemySpawnWeights[i] = enemySpawnWeights[i] / sum;
        }
    }

    void SetTimeUntilSpawn(){
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

    GameObject SpawnEnemy(){
        // Choose enemy index from enemyPrefabs using the weights from enemySpawnWeights
        float random = Random.Range(0f, 1f);
        // Use algorithm as described here to choose random index from enemyPrefabs based off their list of weights
        //   https://forum.unity.com/threads/setting-a-weight-probability-on-a-list-of-objects-ina-datalist-then-choosing-an-object-at-random-b.1463372/
        int i = 0;
        while(true){
            float p = enemySpawnWeights[i];
            if (random < p) break;
            random -= p;
            i++;
        }
        // Instantiate enemy at a random point on the circle
        float angle = Random.Range(0f, 360f);
        float x = transform.position.x + GameManager.Instance.clockRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = transform.position.y + GameManager.Instance.clockRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
        GameObject enemy = Instantiate(enemyPrefabs[i], new Vector2(x,y), Quaternion.identity, enemyParent);
        return enemy;
    }
}

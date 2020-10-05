using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public EnemyBehavior[] enemyTypes;
    public EnemyBehavior boss;

    public float nextSpawn = 5f;
    float spawnTime = 0f;

    float bossSpawnTime = 0f;

    Borders borders;

    void Start()
    {
        borders = FindObjectOfType<Borders>();
        createEnemy(5f);
    }


    public (int startCount, int countMin, int countMax, float timeMin, float timeMax)[] spawnTable = {
        (0, 1, 1, 3f, 3f),
        (5, 1, 2, 3f, 3f),
        (15, 1, 2, 2f, 3f),
        (30, 1, 3, 2f, 3f),
        (40, 2, 3, 2f, 3f),
        (50, 2, 3, 1f, 2f),
        (100, 3, 3, 1f, 2f)
    };

    // Update is called once per frame
    void Update()
    {
        if (EndGame.isGameOver) return;
        spawnTime += Time.deltaTime;
        if (nextSpawn - spawnTime <= 0f) {

            var currentSpawnTable = spawnTable[0];
            for (int i = 1; i < spawnTable.Length; ++i) {
                if (Score.killCount < spawnTable[i].startCount ) continue;
                currentSpawnTable = spawnTable[i];
                break;
            }
            var count = Random.Range(currentSpawnTable.countMin, currentSpawnTable.countMax + 1);
            for (int i = 0; i < count; ++i) {
                createEnemy(nextSpawn / count / 2);
            }

            nextSpawn = Random.Range(currentSpawnTable.timeMin, currentSpawnTable.timeMax);
            spawnTime = 0f;
        }

        if (Score.scoreValue >= 5000) {
            if (bossSpawnTime <= 0f) {
                var pos = new Vector3(Random.value - 0.5f, Random.value - 0.5f).normalized * borders.Diameter() / 2;
                GameObject.Instantiate(boss, pos, Quaternion.identity);
                bossSpawnTime = 30f;
            }
            bossSpawnTime -= Time.deltaTime;
        }

    }

    void createEnemy(float value)
    {
        var pos = new Vector3(Random.value - 0.5f, Random.value - 0.5f).normalized * borders.Diameter() / 2;
        
        var enemy = enemyTypes[Random.Range(0, enemyTypes.Length)];
        var clone = GameObject.Instantiate(enemy, pos, Quaternion.identity);
        clone.GetComponent<EnemyBehavior>().value = value;
        clone.gameObject.SetActive(true);
    }
}

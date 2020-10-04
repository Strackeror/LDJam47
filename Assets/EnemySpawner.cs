using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public EnemyBehavior enemy;

    public float nextSpawn = 5f;
    float spawnTime = 0f;

    Borders borders;

    void Start()
    {
        borders = FindObjectOfType<Borders>();
        createEnemy(5f);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (nextSpawn - spawnTime <= 0f) {

            var maxTime = 5f;
            var maxCount = 2;
            if (Score.scoreValue > 1000) {
                maxTime = 3f;
                maxCount = 3;
            }

            if (Score.scoreValue > 2000) {
                maxTime = 2f;
            }
            var count = Random.Range(1, maxCount);
            for (int i = 0; i < count; ++i) {
                createEnemy(nextSpawn / count);
            }

            nextSpawn = Random.Range(.5f, maxTime);
            spawnTime = 0f;
        }
    }

    void createEnemy(float value)
    {
        var pos = new Vector3(Random.value - 0.5f, Random.value - 0.5f).normalized * borders.Diameter() / 2;
        
        var clone = GameObject.Instantiate(enemy, pos, Quaternion.identity);
        clone.GetComponent<EnemyBehavior>().value = value;
        clone.gameObject.SetActive(true);
    }
}

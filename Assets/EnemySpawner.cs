using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public EnemyBehavior enemy;
    public int lastFrameCreation = 0;

    Borders borders;

    void Start()
    {
        borders = FindObjectOfType<Borders>();
        createEnemy();
       
    }

    // Update is called once per frame
    void Update()
    {
        int cpt = (int)System.Math.Truncate(Time.time);
        if (cpt == lastFrameCreation + 3 && cpt != lastFrameCreation)
        {
            lastFrameCreation = cpt;
            createEnemy();
        }
    }

    void createEnemy()
    {
        Debug.Log("Ennemy Created !");
        var pos = new Vector3(Random.value - 0.5f, Random.value - 0.5f).normalized * borders.Diameter() / 2;
        var clone = GameObject.Instantiate(enemy, pos, Quaternion.identity);
        clone.gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public EnemyBehavior enemy;
    public int lastFrameCreation = 0;

    void Start()
    {
        //EnemyBehavior en1 = Instantiate(enemy, Vector3.zero, Quaternion.identity);
        //Instantiate(enemy, new Vector3(-1f, -1f, 1f), new Quaternion());
        /*var en1 = GameObject.Instantiate(enemy, transform.position + (transform.right * 10), Quaternion.identity);
        Debug.Log(en1);*/
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
        var clone = GameObject.Instantiate(enemy, new Vector3(0f,-3f,0f), Quaternion.identity);
        clone.gameObject.SetActive(true);
    }
}

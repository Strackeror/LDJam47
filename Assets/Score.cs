using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public static int scoreValue = 0;
    Text scoreT;

    // Start is called before the first frame update
    void Start()
    {
        scoreT = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreT.text = "Score: " + scoreValue;
    }
}

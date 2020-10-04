using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{

    static Text gameOverT;
    public static bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOverT = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isGameOver)
            {
                isGameOver = false;
                Score.scoreValue = 0;
                Score.killCount = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public static void GameOver()
    {
        gameOverT.text = "GAME OVER !\n Press fire to restart the game";
        isGameOver = true;
    }
}

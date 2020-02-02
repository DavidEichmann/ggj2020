using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour
{
    private Director _director;

    // Start is called before the first frame update
    void Start()
    {
        _director = FindObjectOfType<Director>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_director.IsGameOver)
        {
            PlayerPrefs.SetString("finalScore", _director.ScoreString(true));
            SceneManager.LoadScene("gameOverScene");
        }
    }
}

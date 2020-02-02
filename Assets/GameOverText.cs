using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        GetComponent<Text>().text = _director.IsGameOver
            ? $"Game Over! {_director.ScoreString(true)}"
            : "";
    }
}

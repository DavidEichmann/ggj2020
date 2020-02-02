using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var dist = PlayerPrefs.GetString("finalScore");
        GetComponent<Text>().text = "Final Distance Travelled: \n" + dist;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

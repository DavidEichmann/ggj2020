using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Odometer : MonoBehaviour
{
    private Director _director;

    public bool Incremental = false;
    public Transform Num1;
    public Transform Num2;
    public Transform Num3;
    public Transform Num4;
    public Transform Num5;

    // Start is called before the first frame update
    void Start()
    {
        _director = FindObjectOfType<Director>();   
    }

    // Update is called once per frame
    void Update()
    {

        RotateNumToCorrectPosition(Num1, 1f);
        RotateNumToCorrectPosition(Num2, 10f);
        RotateNumToCorrectPosition(Num3, 100f);
        RotateNumToCorrectPosition(Num4, 1000f);
        RotateNumToCorrectPosition(Num5, 10000f);
    }

    void RotateNumToCorrectPosition(Transform numObject, float unit)
    {
        if(Incremental)
        {
            numObject.eulerAngles = new Vector3(numObject.eulerAngles.x, numObject.eulerAngles.y, (int)_director.DistanceTravelled / (int)unit % 10 * 36);
        }
        else
        {
            numObject.eulerAngles = new Vector3(numObject.eulerAngles.x, numObject.eulerAngles.y, (int)_director.DistanceTravelled / unit % 10 * 36);
        }
    }
}

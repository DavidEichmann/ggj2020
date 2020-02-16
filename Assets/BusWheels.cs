using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BusWheels : MonoBehaviour
{
    public float SpeedFactor = 1f;
    public EngineWheel FrontRight;
    public EngineWheel FrontLeft;
    public EngineWheel BackRight;
    public EngineWheel BackLeft;

    private Director _director;

    // Start is called before the first frame update
    void Start()
    {
        _director = FindObjectOfType<Director>();
    }

    // Update is called once per frame
    void Update()
    {
        //These wheels are rotated so need to spin backwards
        FrontRight.RotateSpeed = -_director.BusMeterPerSec * SpeedFactor;
        BackRight.RotateSpeed = -_director.BusMeterPerSec * SpeedFactor;

        FrontLeft.RotateSpeed = _director.BusMeterPerSec * SpeedFactor;
        BackLeft.RotateSpeed = _director.BusMeterPerSec * SpeedFactor;
    }
}

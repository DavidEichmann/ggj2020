using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;

public class EngineWheel : MonoBehaviour
{

    public bool Active = true;
    public float RotateSpeed;
    public Axis Axis = Axis.Z;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Active)
        {
            if(Axis == Axis.X)
            {
                transform.Rotate(RotateSpeed * Time.deltaTime, 0, 0);

            }
            else if (Axis == Axis.Y)
            {
                transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);

            }
            else if (Axis == Axis.Z)
            {
                transform.Rotate(0, 0, RotateSpeed * Time.deltaTime);

            }
        }
    }

    public void ActivateWheel()
    {
        Active = true;
    }

    public void DeactivateWheel()
    {
        Active = false;
    }
}

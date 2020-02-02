using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EngineWheel : MonoBehaviour
{

    public bool Active = true;
    public float RotateSpeed;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Active)
        {
            transform.Rotate(0, 0, RotateSpeed * Time.deltaTime);
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

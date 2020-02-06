﻿using UnityEngine;

public class ZoomController : MonoBehaviour
{
    public float zoomedInPosition;
    public float zoomedOutPosition;

    private float _smoothFactor;
    private Vector3 _targetPosition;

    private Director _director;

    private void Awake()
    {
        _director = FindObjectOfType<Director>();
    }

    private void Start()
    {
        _smoothFactor = 2;
    }

    private void Update()
    {
        if (!_director.IsGameStarted)
        {
            return;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * _smoothFactor);
        if (Input.GetKey(KeyCode.Joystick1Button4))
        {
            ZoomOut();
        }
        else
        {
            ZoomIn();
        }
    }

    public void ZoomIn()
    {
        //if (transform.position.z == zoomedInPosition) return;
        var pos = transform.localPosition;
        pos.z = zoomedInPosition;
        _targetPosition = pos;
    }

    public void ZoomOut()
    {
        //if (transform.position.z == zoomedOutPosition) return;
        var pos = transform.localPosition;
        pos.z = zoomedOutPosition;
        _targetPosition = pos;
    }
}

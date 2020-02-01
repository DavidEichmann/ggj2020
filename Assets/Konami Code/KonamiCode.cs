using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum KonamiKeyCode
{
    A, B,
    X, Y,
    Up, Down,
    Left, Right
}

public class KonamiCode : MonoBehaviour
{
    public UnityEvent OnFailure;
    public UnityEvent OnSuccess;

    public int length = 3;

    private static readonly Array _values = Enum.GetValues(typeof(KonamiKeyCode));

    private Queue<KonamiKeyCode> _remainingCode = new Queue<KonamiKeyCode>();
    private TrafficLight _trafficLight;

    private void OnEnable()
    {
        // TODO: Disable character movement
    }

    private void OnDisable()
    {
        _remainingCode.Clear();
    }

    private void Awake()
    {
        _trafficLight = GetComponent<TrafficLight>();
    }

    private void Update()
    {
        if (_remainingCode.Count == 0)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                Generate();
            }
            return;
        }

        if (Input.anyKeyDown || DPadButtons.AnyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.R))
            {
                CheckCode(KonamiKeyCode.A);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.F))
            {
                CheckCode(KonamiKeyCode.B);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Q))
            {
                CheckCode(KonamiKeyCode.X);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.E))
            {
                CheckCode(KonamiKeyCode.Y);
                return;
            }
            if (DPadButtons.IsUp || Input.GetKeyDown(KeyCode.W))
            {
                CheckCode(KonamiKeyCode.Up);
                return;
            }
            if (DPadButtons.IsDown || Input.GetKeyDown(KeyCode.S))
            {
                CheckCode(KonamiKeyCode.Down);
                return;
            }
            if (DPadButtons.IsLeft || Input.GetKeyDown(KeyCode.A))
            {
                CheckCode(KonamiKeyCode.Left);
                return;
            }
            if (DPadButtons.IsRight || Input.GetKeyDown(KeyCode.D))
            {
                CheckCode(KonamiKeyCode.Right);
                return;
            }

            // Error if any other button is pressed
            Error();
        }
    }

    private void CheckCode(KonamiKeyCode keyCode)
    {
        if (keyCode == _remainingCode.Peek())
        {
            _remainingCode.Dequeue();
            if (_remainingCode.Count == 0)
            {
                Success();
            }
            else
            {
                Debug.Log(_remainingCode.Peek());
            }
        }
        else
        {
            Error();
        }
    }

    private void Error()
    {
        Generate();
        OnFailure.Invoke();
    }

    private void Success()
    {
        _trafficLight.TryRepair();
        OnSuccess.Invoke();
    }

    public void Generate()
    {
        var code = new KonamiKeyCode[length];
        for (var i = 0; i < length; i++)
        {
            code[i] = (KonamiKeyCode)_values.GetValue(UnityEngine.Random.Range(0, _values.Length));
        }
        _remainingCode = new Queue<KonamiKeyCode>(code);
        Debug.Log(_remainingCode.Peek());
    }
}

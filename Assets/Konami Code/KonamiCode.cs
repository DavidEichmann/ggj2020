using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum KonamiKeyCode
{
    A,
    B,
    X,
    Y,
    Left,
    Up,
    Right,
    Down,
}

public class KonamiCode : MonoBehaviour
{
    public UnityEvent OnFailure;
    public UnityEvent OnSuccess;

    public int length = 3;
    [SerializeField] private NeonLightManager neonLightManager;

    private static readonly Array _values = Enum.GetValues(typeof(KonamiKeyCode));

    private Queue<KonamiKeyCode> _remainingCode = new Queue<KonamiKeyCode>();
    private TrafficLight _trafficLight;

    private void OnEnable()
    {
        Generate();
        PigPlayerController.konamiMode = true;
    }

    IEnumerator FlashAllError()
    {
        neonLightManager.SwitchAll(true);
        yield return new WaitForSeconds(0.5f);
        neonLightManager.SwitchAll(false);
        Generate();
    }

    private void OnDisable()
    {
        _remainingCode.Clear();
        PigPlayerController.konamiMode = false;
        neonLightManager.SwitchAll(false);
    }

    private void Awake()
    {
        _trafficLight = GetComponent<TrafficLight>();
    }

    private void Update()
    {
        if (_remainingCode.Count == 0)
        {
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
                neonLightManager.OnlyOne(_remainingCode.Peek());
            }
        }
        else
        {
            Error();
        }
    }

    private void Error()
    {
        StartCoroutine("FlashAllError");
        OnFailure.Invoke();
    }

    private void Success()
    {
        neonLightManager.SwitchAll(false);
        _trafficLight.TryRepair();
        PigPlayerController.konamiMode = false;
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
        neonLightManager.OnlyOne(_remainingCode.Peek());
    }
}

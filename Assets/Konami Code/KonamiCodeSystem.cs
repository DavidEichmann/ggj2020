using System.Collections.Generic;
using UnityEngine;

public class KonamiCodeSystem : MonoBehaviour
{
    public delegate void FailAction();
    public static event FailAction OnFail;

    public delegate void SuccessAction();
    public static event SuccessAction OnSuccess;

    private static Queue<KonamiKeyCode> _remainingCode = new Queue<KonamiKeyCode>();

    public static void AddCode(KonamiKeyCode[] code)
    {
        _remainingCode = new Queue<KonamiKeyCode>(code);
        Debug.Log(_remainingCode.Peek());
    }

    private void Awake()
    {
        gameObject.AddComponent<DPadButtons>();
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
        _remainingCode.Clear();
        OnFail?.Invoke();
    }

    private void Success()
    {
        OnSuccess?.Invoke();
    }
}

using System;
using UnityEngine;

public enum KonamiKeyCode
{
    A, B,
    X, Y,
    Up, Down,
    Left, Right
}

public class KonamiCodeGenerator : MonoBehaviour
{
    private static readonly Array _values = Enum.GetValues(typeof(KonamiKeyCode));

    private bool _started = false;
    private int _length = 0;

    private void OnEnable()
    {
        KonamiCodeSystem.OnFail += Generate;
        KonamiCodeSystem.OnSuccess += Success;
    }

    private void OnDisable()
    {
        KonamiCodeSystem.OnFail -= Generate;
        KonamiCodeSystem.OnSuccess -= Success;
    }

    public void Generate(int length)
    {
        if (_started)
        {
            return;
        }

        _started = true;
        _length = length;
        Generate();
    }

    private void Generate()
    {
        Debug.Log("Generate");
        var code = new KonamiKeyCode[_length];
        for (var i = 0; i < _length; i++)
        {
            code[i] = (KonamiKeyCode)_values.GetValue(UnityEngine.Random.Range(0, _values.Length));
        }
        KonamiCodeSystem.AddCode(code);
    }

    private void Success()
    {
        Debug.Log("Success");
        _started = false;
    }
}

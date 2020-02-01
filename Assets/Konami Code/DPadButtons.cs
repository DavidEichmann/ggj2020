using UnityEngine;

public class DPadButtons : MonoBehaviour
{
    public static bool IsLeft;
    public static bool IsRight;
    public static bool IsUp;
    public static bool IsDown;
    public static bool AnyKeyDown => IsLeft || IsRight || IsUp || IsDown;

    private float _lastX;
    private float _lastY;

    private void Update()
    {
        float x = Input.GetAxis("DPadX");
        float y = Input.GetAxis("DPadY");

        IsLeft = false;
        IsRight = false;
        IsUp = false;
        IsDown = false;

        if (_lastX != x)
        {
            if (x == -1)
            {
                IsLeft = true;
            }
            else if (x == 1)
            {
                IsRight = true;
            }
        }

        if (_lastY != y)
        {
            if (y == -1)
            {
                IsDown = true;
            }
            else if (y == 1)
            {
                IsUp = true;
            }
        }

        _lastX = x;
        _lastY = y;
    }
}

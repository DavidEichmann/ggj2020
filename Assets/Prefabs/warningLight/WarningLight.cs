using System.Collections;
using UnityEngine;

public class WarningLight : MonoBehaviour
{
    private Color _color;
    private Light _light;
    private Renderer _rendererForEmission;

    void Awake()
    {
        _light = GetComponentInChildren<Light>();
        _rendererForEmission = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color)
    {
        StopAllCoroutines();
        _color = color;
        On();
    }

    private void On()
    {
        _light.color = _color;
        _rendererForEmission.material.SetColor("_EmissionColor", _color);
    }

    private void Off()
    {
        _light.color = Color.black;
        _rendererForEmission.material.SetColor("_EmissionColor", Color.black);
    }

    public void StartFlashSlow()
    {
        StopAllCoroutines();
        StartCoroutine("FlashSlow");
    }
    public IEnumerator FlashSlow()
    {
        while (true)
        {
            On();
            yield return new WaitForSeconds(1);
            Off();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void StartFlashQuick()
    {
        StopAllCoroutines();
        StartCoroutine("FlashQuick");
    }
    public IEnumerator FlashQuick()
    {
        while (true)
        {
            On();
            yield return new WaitForSeconds(0.5f);
            Off();
            yield return new WaitForSeconds(0.2f);
        }
    }
}

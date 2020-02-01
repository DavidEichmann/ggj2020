using UnityEngine;

public class WarningLight : MonoBehaviour
{
    public Color StartColor;
    private Renderer _rendererForEmission;
    private Light _light;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponentInChildren<Light>();
        _rendererForEmission = GetComponent<MeshRenderer>();
        _light.color = StartColor;
        _rendererForEmission.material.SetColor("_EmissionColor", StartColor);
    }

    public void SetColor(string colorString)
    {
        if (ColorUtility.TryParseHtmlString(colorString, out var color))
        {
            SetColor(color);
        }
    }

    public void SetColor(Color color)
    {
        _light.color = color;
        _rendererForEmission.material.SetColor("_EmissionColor", color);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarningLight : MonoBehaviour
{
    public Renderer[] RendererForEmission;
    private List<Material> _emissionMaterial;
    private Light _light;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponentInChildren<Light>();
        if (RendererForEmission.Any())
        {
            _emissionMaterial = new List<Material>();
            foreach (var renderer in RendererForEmission)
            {
                _emissionMaterial.Add(renderer.material);
            }
        }
    }

    public void SetColor(string colorString)
    {
        if (ColorUtility.TryParseHtmlString(colorString, out var color))
        {           
            _light.color = color;
            if (_emissionMaterial.Any())
            {
                _emissionMaterial.ForEach(f => f.SetColor("_EmissionColor", color));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

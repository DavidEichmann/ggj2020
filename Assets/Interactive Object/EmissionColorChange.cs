using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Interactive_Object
{

    public class EmissionColorChange : MonoBehaviour
    {
        public Color StartColor;
        private Renderer[] _rendererForEmission;
        private List<Material> _emissionMaterial;
        private Light _light;

        // Start is called before the first frame update
        void Start()
        {
            _light = GetComponentInChildren<Light>();
            _rendererForEmission = GetComponentsInChildren<MeshRenderer>();

            if (_rendererForEmission.Any())
            {
                _emissionMaterial = new List<Material>();
                foreach (var renderer in _rendererForEmission)
                {
                    renderer.material.SetColor("_EmissionColor", StartColor);
                }
            }

            if (_light != null)
            {
                _light.color = StartColor;
            }
         
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

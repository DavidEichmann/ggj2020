using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonLight : MonoBehaviour
{

    private Renderer renderer;
    public Color nColor;

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.material.SetColor("_EmissionColor", nColor);
    }

    public void SwitchLight(bool turnOn)
    {
        if(turnOn)
        {
            renderer.material.SetColor("_EmissionColor", nColor);
        }
        else{
            renderer.material.SetColor("_EmissionColor", Color.black);
        }
    }

    public void PlaySound()
    {
        GetComponent<AudioSource>()?.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }

}

// public enum NeonColour{
//     BLUE, YELLOW, GREEN, RED, PURPLE
// }

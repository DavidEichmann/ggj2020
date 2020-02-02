using System.Collections.Generic;
using UnityEngine;

public class NeonLightManager : MonoBehaviour
{
    public List<NeonLight> neonLights;

    private void Start()
    {
        SwitchAll(false);
    }

    public void SwitchAll(bool turnOn)
    {
        foreach (var l in neonLights)
        {
            l.SwitchLight(turnOn);
        }
    }

    public void OnlyOne(KonamiKeyCode keyCode) => OnlyOne((int)keyCode);
    public void OnlyOne(int lightIndex)
    {
        for (var i = 0; i < neonLights.Count; i++)
        {
            neonLights[i].SwitchLight(i == lightIndex);
        }
    }
}

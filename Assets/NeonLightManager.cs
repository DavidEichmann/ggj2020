using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonLightManager : MonoBehaviour
{
    public List<NeonLight> neonLights;

    // Start is called before the first frame update
    void Start()
    {
        SwitchAll(false);
        StartCoroutine("CycleButtons");
    }

    public void SwitchAll(bool turnOn){
        foreach(var l in neonLights){
            l.SwitchLight(turnOn);
        }
    }

    public void AllButOne(int lightIndex){
        for(var i =0; i < neonLights.Count; i++){
            neonLights[i].SwitchLight(i == lightIndex);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CycleButtons(){
        var currentLight = 0;
        while(true){
            AllButOne(currentLight);
            currentLight = (currentLight + 1) % neonLights.Count;
            yield return new WaitForSeconds(1);
        }

    }
}

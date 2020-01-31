using UnityEngine;
using System.Collections;
using System;



public class GameObjectActivator : MonoBehaviour
{
 
    public void Deactivate(GameObject gameObj)
    {
        gameObj.SetActive(false);        
    }

    public void Activate(GameObject gameObj)
    {
        gameObj.SetActive(true);
    }
}

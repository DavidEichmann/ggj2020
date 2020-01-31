using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public delegate void InteractEvent(GameObject obj);



public class InteractiveObject : MonoBehaviour
{ 
    private List<Shader> oldShaders = new List<Shader>();
    private bool shaderApplied = false;
    private GameObject _player;

    public int Distance;
    public UnityEvent OnClick;


    void Awake()
    {
        _player = GameObject.FindGameObjectsWithTag("Player")[0];
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            oldShaders.Add(item.material.shader);
        }
    }

    void Update()
    {

    }

    public void OnMouseUpAsButton()
    {
        if ((transform.position - _player.transform.position).magnitude < Distance)
        {

            if(OnClick != null)
            {
                OnClick.Invoke();
            }
        }
    }

    private void OnMouseOver()
    {
        if ((transform.position - _player.transform.position).magnitude < Distance)
        {
            shaderApplied = true;
            var items = GetComponentsInChildren<Renderer>();
            for (var i = 0; i < items.Length; ++i)
            {
                items[i].material.shader = Shader.Find("HighlightEffect");
            }
        }
        else
        {
            if (shaderApplied)
            {
                shaderApplied = false;
                var items = GetComponentsInChildren<Renderer>();
                for (var i = 0; i < items.Length; ++i)
                {
                    items[i].material.shader = oldShaders[i];
                }
            }
        }
    }

    private void OnMouseExit()
    {
        if (shaderApplied)
        {
            shaderApplied = false;
            var items = GetComponentsInChildren<Renderer>();
            for (var i = 0; i < items.Length; ++i)
            {
                items[i].material.shader = oldShaders[i];
            }
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Retry : MonoBehaviour
{
    public NeonLight NeonA;
    public Color Color;
    public bool IsOn = false;

    public AudioSource FailureAudio;
    public AudioSource SuccessAudio;

    private Renderer _renderer;
    private Director _director;

    // Start is called before the first frame update
    void Awake()
    {
        _director = FindObjectOfType<Director>();
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material.SetColor("_EmissionColor", Color.black);
    }

    public void SwitchOn()
    {
        IsOn = true;
        _renderer.material.SetColor("_EmissionColor", Color);
        NeonA.SwitchLight(true);
    }
    private void Error()
    {
        StartCoroutine(FlashError());
        FailureAudio?.GetComponent<AudioSource>()?.Play();
    }
    private IEnumerator FlashError()
    {
        NeonA.SwitchLight(false);
        yield return new WaitForSeconds(0.5f);
        NeonA.SwitchLight(true);
    }

    private void Success()
    {
        SuccessAudio?.GetComponent<AudioSource>()?.Play();
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(2f);
        _director.RestartGame();
    }

    private void Start()
    {
        NeonA.SwitchLight(false);
    }

    private void Update()
    {
        if (IsOn)
        {
            if (Input.anyKeyDown || DPadButtons.AnyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.R))
                {
                    Success();
                    return;
                }
                Error();
            }
        } 
    }

}
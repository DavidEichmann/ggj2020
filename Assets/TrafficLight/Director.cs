using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Director : MonoBehaviour
{
    public UnityEvent OnGameOver;

    public float? GameOverTime { get; private set; } = null;

    public bool IsGameOver => GameOverTime.HasValue;

    public float Score => GameOverTime ?? Time.time;

    private TrafficLight[] _trafficLights;

    // Start is called before the first frame update
    void Start()
    {
        _trafficLights = FindObjectsOfType<TrafficLight>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameOver)
        {
            bool allTrafficLightsAreRed = _trafficLights
                .All(trafficLight => trafficLight.State == TrafficLightState.Red);
            if (allTrafficLightsAreRed)
            {
                GameOverTime = Time.time;
                OnGameOver.Invoke();
            }
        }
    }
}

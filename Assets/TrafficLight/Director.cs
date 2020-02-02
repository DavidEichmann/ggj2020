using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Director : MonoBehaviour
{
    public UnityEvent OnGameOver;

    // Time at which the game was lost. Null if not lost yet
    public float? GameOverTime { get; private set; } = null;

    public bool IsGameOver => GameOverTime.HasValue;

    // Health between 0 and 1. 1 means full health.
    public float Health = 1;

    // Damage per second that a red a traffic light does.
    // Guideline is that at full health and all lights red, you'll have about 10 seconds to lose all health.
    // There are 5 lights, so thats 50 seconds to lose all (1.0) health in 50 seconds from a single red light.
    public float RedDamageRate = 1.0f  / 50.0f;

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
            int numRedLights = _trafficLights.Count(trafficLight => trafficLight.State == TrafficLightState.Red);
            float healthLossRate = numRedLights * RedDamageRate;
            float damage = Time.deltaTime * healthLossRate;
            Health = Mathf.Max(0, Health - damage);

            if (Health <= 0)
            {
                GameOverTime = Time.time;
                OnGameOver.Invoke();
            }
        }
    }
    public string ScoreString(bool subseconds)
    {
        float score = Score;
        int mins = Mathf.FloorToInt(score / 60);
        string minsStr = mins > 0 ? "" : $"{mins}:";
        float secs = score % 60;
        return minsStr + (subseconds ? $"{secs:F2}" : $"{Mathf.FloorToInt(secs):D2}");
    }
}

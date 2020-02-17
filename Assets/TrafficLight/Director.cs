using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    public UnityEvent OnGameOver;
    public PigPlayerController player;

    public TerrainController Terrain;

    public float GameOverDecelaration;

    // Time at which the game was lost. Null if not lost yet
    public float? GameOverTime { get; private set; } = null;
    public float? GameStartTime { get; private set; } = null;
    public float GameTime => IsGameStarted ? Time.time - GameStartTime.Value : 0;

    public bool IsGameOver => GameOverTime.HasValue;
    public bool IsGameStarted => GameStartTime.HasValue;

    // Health between 0 and 1. 1 means full health.
    public float Health = 1;

    // Damage per second that a red a traffic light does.
    // Guideline is that at full health and all lights red, you'll have about 10 seconds to lose all health.
    // There are 5 lights, so thats 50 seconds to lose all (1.0) health in 50 seconds from a single red light.
    public float RedDamageRate = 1.0f / 50.0f;

    // Speed of the bus in m / s
    public float BusSpeedKph = 100;

    // Distance travelled in meters (stops increasing on game over).
    public float DistanceTravelled = 0;
    public float BusMeterPerSec = 0;

    // Meters traveled rounded down.
    public int Score => Mathf.FloorToInt(DistanceTravelled);

    private TrafficLight[] _trafficLights;
    private bool _hasGameOverEventHappened = false;

    public void StartGame()
    {
        player.enabled = true;
        GameStartTime = Time.time;
    }

    public void RestartGame()
    {
        //Restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Start is called before the first frame update
    void Start()
    {
        player.enabled = false;
        _trafficLights = FindObjectsOfType<TrafficLight>();

    }

    // Update is called once per frame
    void Update()
    {
        float BusSpeedMph = BusSpeedKph * 1000;
        BusMeterPerSec = BusSpeedMph / (60 * 60);
        DistanceTravelled += BusMeterPerSec * Time.deltaTime;

        if (!IsGameStarted)
        {
            return;
        }

        if (!IsGameOver)
        {
            // Score
       

            // Health
            int numRedLights = _trafficLights.Count(trafficLight => trafficLight.State == TrafficLightState.Red);
            float healthLossRate = numRedLights * RedDamageRate;
            float damage = Time.deltaTime * healthLossRate;
            Health = Mathf.Max(0, Health - damage);

            // Check game over.
            if (Health <= 0)
            {
                GameOverTime = GameTime;
                if (!_hasGameOverEventHappened)
                {
                    OnGameOver.Invoke();
                    _hasGameOverEventHappened = true;
                }
            }
        }

        if (IsGameOver)
        {
            BusSpeedKph = Mathf.Max(0, BusSpeedKph - (GameOverDecelaration * Time.deltaTime));
        }

    }
    public string ScoreString(bool subseconds)
        => $"{Score} m";
}

﻿using UnityEngine;
using UnityEngine.Events;

public enum TrafficLightState
{
    // Green, moving to Amber at GreenToAmberRateSeconds.
    Green,

    // Amber, moving to Red at AmberToRedRateSeconds.
    Amber,

    // Broken!
    Red
}

[System.Serializable]
public class TrafficLightEvent : UnityEvent<GameObject> { }

public class TrafficLight : MonoBehaviour
{
    //
    // Fields
    //

    public TrafficLightState State { get; private set; }

    // Minimum time spent in green state before changing to Amber.
    public float MinGreenTime = 5;

    // Expected seconds to change from Green to Amber.
    public float GreenToAmberRateSeconds = 10;

    // Minimum time spent in amber state before changing to Red
    public float MinAmberTime = 10;

    // Expected seconds to change from Amber to Red after MinAmberTime.
    public float AmberToRedRateSeconds = 5;

    // When Paused is true, no transitions will happen.
    public bool Paused = true;

    // Game time when this gets automatically unpaused
    public float StartTime = 20;

    private float? UnpauseTime;

    public Color green;
    public Color amber;
    public Color red;

    // On state change event.
    public TrafficLightEvent OnGreenToAmber;
    public TrafficLightEvent OnAmberToRed;
    public TrafficLightEvent BrokenToGreen;

    // Expected time from Green to Red.
    public float ExpectedGreenToRedSeconds
        => GreenToAmberRateSeconds + MinAmberTime + AmberToRedRateSeconds;

    //
    // Properties
    //

    private WarningLight _warningLight;
    private KonamiCode _konamiCode;
    private Director _director;
    private bool _insideBounds = false;

    // Start time we enetered Amber state.
    // Only valid when `State == Amber`
    private float __AmberStartTime;
    private float AmberStartTime
    {
        get
        {
            if (State != TrafficLightState.Amber)
            {
                Debug.LogError("Trying to get AmberStartTime, but State is not Amber, it is: " + State);
            }

            return __AmberStartTime;
        }

        set
        {
            if (State != TrafficLightState.Amber)
            {
                Debug.LogError("Trying to set AmberStartTime, but State is not Amber, it is: " + State);
            }

            __AmberStartTime = value;
        }
    }

    // Start time we enetered Green state.
    // Only valid when `State == Green`
    private float __GreenStartTime;
    private float GreenStartTime
    {
        get
        {
            if (State != TrafficLightState.Green)
            {
                Debug.LogError("Trying to get GreenStartTime, but State is not Green, it is: " + State);
            }

            return __GreenStartTime;
        }

        set
        {
            if (State != TrafficLightState.Green)
            {
                Debug.LogError("Trying to set GreenStartTime, but State is not Green, it is: " + State);
            }

            __GreenStartTime = value;
        }
    }

    private void Awake()
    {
        _warningLight = GetComponent<WarningLight>();
        _konamiCode = GetComponent<KonamiCode>();
        _director = FindObjectOfType<Director>();
    }

    private void Start()
    {
        UnpauseTime = StartTime;
        State = TrafficLightState.Green;
        GreenStartTime = _director.GameTime;
        _warningLight.SetColor(green);
    }

    // Update is called once per frame
    void Update()
    {
        if (UnpauseTime.HasValue && _director.GameTime > UnpauseTime)
        {
            Paused = false;
            UnpauseTime = null;
        }

        switch (State)
        {
            case TrafficLightState.Green:
                float timeSinceEndSafeGreen = _director.GameTime - (GreenStartTime + MinGreenTime);
                if (timeSinceEndSafeGreen > 0 && RandomOccurence(GreenToAmberRateSeconds))
                {
                    State = TrafficLightState.Amber;
                    AmberStartTime = _director.GameTime;
                    _warningLight.SetColor(amber);
                    if (_insideBounds) EnableKonami();
                    OnGreenToAmber.Invoke(gameObject);
                }

                break;

            case TrafficLightState.Amber:
                float timeSinceEndSafeAmber = _director.GameTime - (AmberStartTime + MinAmberTime);
                if (timeSinceEndSafeAmber > 0 && RandomOccurence(AmberToRedRateSeconds))
                {
                    State = TrafficLightState.Red;
                    _warningLight.SetColor(red);
                    if (_insideBounds) EnableKonami();
                    OnAmberToRed.Invoke(gameObject);
                }

                break;

            case TrafficLightState.Red:
                // Do Nothing
                break;
        }
    }

    // Repair this traffic light.
    // Has no effect in Green state.
    public void TryRepair()
    {
        switch (State)
        {
            case TrafficLightState.Green:
                break;
            case TrafficLightState.Amber:
            case TrafficLightState.Red:
                State = TrafficLightState.Green;
                GreenStartTime = _director.GameTime;
                _warningLight.SetColor(green);
                BrokenToGreen.Invoke(gameObject);
                break;
        }
    }

    // Return true if an event is happening (random).
    // Assumes this is called over a Time.deltaTime time period.
    private bool RandomOccurence(float rateSeconds)
    {
        if (Paused)
        {
            return false;
        }

        // Convert from seconds per occurence to occurences per second.
        float occurencesPerSecond = 1 / rateSeconds;

        // This is based on a poisson distribution.
        // See https://en.wikipedia.org/wiki/Poisson_distribution#Probability_of_events_for_a_Poisson_distribution

        // Time interval.
        float seconds = Time.deltaTime;

        // Average number of occurences per interval
        float lambda = occurencesPerSecond * seconds;

        // This is: 1 - P( 0 events in 1 time interval )
        float probOfNoEvents = 1 / Mathf.Exp(lambda);

        return Random.value > probOfNoEvents;

    }

    public void EnableKonami()
    {
        _insideBounds = true;
        if (State != TrafficLightState.Green)
        {
            _konamiCode.enabled = true;
        }
    }

    public void DisableKonami()
    {
        _insideBounds = false;
        _konamiCode.enabled = false;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

public class TrafficLight : MonoBehaviour
{
    //
    // Fields
    //

    // Controlled by TrafficLightManager.
    public TrafficLightState State = TrafficLightState.Green;

    // Expected seconds to change from Green to Amber.
    public float GreenToAmberRateSeconds = 15;

    // Minimum time spent in amber state before changing to Red
    public float MinAmberTime = 10;

    // Expected seconds to change from Amber to Red after MinAmberTime.
    public float AmberToRedRateSeconds = 5;

    // On state change event.
    public UnityEvent OnGreenToAmber;
    public UnityEvent OnAmberToRed;
    public UnityEvent OnAmberToGreen;

    // Expected time from Green to Red.
    public float ExpectedGreenToRedSeconds
        => GreenToAmberRateSeconds + MinAmberTime + AmberToRedRateSeconds;


    //
    // Properties
    //

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

    // Start is called before the first frame update
    void Start()
    {
        OnGreenToAmber = OnGreenToAmber ?? new UnityEvent();
        OnAmberToRed   = OnAmberToRed   ?? new UnityEvent();
        OnAmberToGreen = OnAmberToGreen ?? new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case TrafficLightState.Green:
                if (RandomOccurence(GreenToAmberRateSeconds))
                {
                    var oldState = State;
                    State = TrafficLightState.Amber;
                    AmberStartTime = Time.time;
                    OnGreenToAmber.Invoke();
                }

                break;

            case TrafficLightState.Amber:
                float timeSinceEndSafeAmber = Time.time - (AmberStartTime + MinAmberTime);
                if (timeSinceEndSafeAmber > 0 && RandomOccurence(AmberToRedRateSeconds))
                {
                    State = TrafficLightState.Red;
                    OnAmberToRed.Invoke();
                }

                break;

            case TrafficLightState.Red:
                // Do Nothing
                break;
        }
    }

    // Repair this traffic light.
    // Only has effect if in amber state.
    public void TryRepair()
    {
        if (State == TrafficLightState.Amber)
        {
            State = TrafficLightState.Green;
            OnAmberToGreen.Invoke();
        }
    }

    // Return true if an event is happening (random).
    // Assumes this is called over a Time.deltaTime time period.
    private bool RandomOccurence(float rateSeconds)
    {
        // Convert from  secnods per occurence to occurences per second.
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
}
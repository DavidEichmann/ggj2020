using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverCamera : MonoBehaviour
{
    public float duration;
    public Transform BusCamPosition;
    public Transform OdometerCamPosition;
    public UnityEvent DoneEvent;

    private float _startTime;
    private Vector3 _startPosition;

    void OnEnable()
    {
        _startTime = Time.time;
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var t = Time.time - _startTime;

        int n = 3;
        var sectionTime = duration / n;
        float sectionT;
        Vector3 sectionStart;
        Vector3 sectionEnd;
        if (t < sectionTime)
        {
            // Move to bus cam position.
            sectionT = t;
            sectionStart = _startPosition;
            sectionEnd = BusCamPosition.position;
        }
        else if (t < sectionTime * 2)
        {
            // Keep looking at the bus.
            sectionT = (t / sectionTime) - 2;
            sectionStart = BusCamPosition.position;
            sectionEnd = BusCamPosition.position;
        }
        else
        {
            // Move to bus cam position.
            sectionT = (t / sectionTime) - 1;
            sectionStart = BusCamPosition.position;
            sectionEnd = OdometerCamPosition.position;

            if (sectionT > 1)
            {
                // Done!
                DoneEvent.Invoke();
                this.enabled = false;
            }
        }

        Vector3.Lerp(sectionStart, sectionEnd, sectionT);
    }
}

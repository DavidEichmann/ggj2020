using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
    public List<Vector3> targetOffsets; // possitions of each "offset"
    public List<float> panTimeWeights; // Weight of each section (between offsets). Length is = targetOffsets.Count - 1
    public float totalPanTime;

    private float _startTime; // of the last offets
    private Director _director;
    private CameraFollower _camFollower;

    public Transform player;
    private int currentOffset = 0;

    public bool skipPan = false;

    private void Awake()
    {
        _director = FindObjectOfType<Director>();
        _camFollower = GetComponent<CameraFollower>();
    }

    private void Start()
    {
        _startTime = Time.time;
    }

    private void Update()
    {
        // If done, stop!
        if(skipPan){
            _director.StartGame();
            _camFollower.enabled = true;
            this.enabled = false;
            return;
        }

        var currentPanTime = totalPanTime * panTimeWeights[currentOffset] / panTimeWeights.Sum();
        var prevPos = player.position + targetOffsets[currentOffset];
        var targetPos = player.position + targetOffsets[currentOffset + 1];
        var fracComplete = (Time.time - _startTime) / currentPanTime;
        transform.position = Vector3.Slerp(prevPos, targetPos, fracComplete);
        transform.LookAt(player.position);

        if (fracComplete >= 1)
        {
            currentOffset += 1;
            _startTime = Time.time;
        }

        if(currentOffset + 1 == targetOffsets.Count){
            _director.StartGame();
            _camFollower.enabled = true;
            this.enabled = false;
        }
    }
}

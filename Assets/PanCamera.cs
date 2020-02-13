using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
    public List<Vector3> targetOffsets;
    private float _startTime;
    private Director _director;
    private CameraFollower _camFollower;

    public Transform player;
    public float secondsToPan;
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

        if(skipPan){
            _director.StartGame();
            _camFollower.enabled = true;
            this.enabled = false;
            return;
        }

        var prevPos = player.position + targetOffsets[currentOffset];
        var targetPos = player.position + targetOffsets[currentOffset + 1];
        var fracComplete = (Time.time - _startTime) / secondsToPan;
        transform.position = Vector3.Slerp(prevPos, targetPos, fracComplete);
        transform.LookAt(player.position);

        if (transform.position == targetPos)
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

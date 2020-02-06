using UnityEngine;

public class PanCamera : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private Vector3 _lookAtPosition;
    private float _startTime;
    private Director _director;

    public Transform targetTransform;
    public Transform lookAtTransform;
    public float secondsToPan;
    public Camera mainCamera;

    private void Awake()
    {
        _initialPosition = transform.position;
        _targetPosition = targetTransform.position;
        _lookAtPosition = lookAtTransform.position;
        _director = FindObjectOfType<Director>();
    }

    private void Start()
    {
        _startTime = Time.time;
    }

    private void Update()
    {
        var fracComplete = (Time.time - _startTime) / secondsToPan;
        transform.position = Vector3.Slerp(_initialPosition, _targetPosition, fracComplete);
        transform.LookAt(_lookAtPosition);

        // TODO: Fix weird camera switch
        if (transform.position == _targetPosition)
        {
            mainCamera.enabled = true;
            _director.StartGame();
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class TerrainController : MonoBehaviour
{
    public float SpeedFactor = 1f;

    private float _speed;
    private Vector3 _initialPos;
    private float _length;
    private float _createX;
    private bool _created;
    private Director _director;
    private void Awake()
    {
        _initialPos = transform.position;
    }

    private void Start()
    {
        _director = FindObjectOfType<Director>();
        var children = GetComponentsInChildren<MeshRenderer>();
        foreach (var child in children)
        {
            _length += child.bounds.size.x;
        }
        _createX = _length / children.Length;
    }

    private void Update()
    {
        _speed = _director.BusMeterPerSec * SpeedFactor;

        transform.Translate(new Vector3(_speed, 0) * Time.deltaTime);

        if (!_created && transform.position.x > _createX)
        {
            Create(transform.position.x - _length);
        }
        if (_created && transform.position.x > _length)
        {
            Destroy(gameObject);
        }
    }

    private void Create(float x)
    {
        _created = true;
        _initialPos.x = x;
        Instantiate(gameObject, _initialPos, Quaternion.identity);
    }
}

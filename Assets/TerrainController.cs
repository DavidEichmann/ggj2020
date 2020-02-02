using UnityEngine;

public class TerrainController : MonoBehaviour
{
    public float speed;

    private Vector3 initialPos;
    private float length;
    private float createX;
    private bool created;

    private void Awake()
    {
        initialPos = transform.position;
    }

    private void Start()
    {
        var children = GetComponentsInChildren<MeshRenderer>();
        foreach (var child in children)
        {
            length += child.bounds.size.x;
        }
        createX = length / children.Length;
    }

    private void Update()
    {
        transform.Translate(new Vector3(speed, 0) * Time.deltaTime);

        if (!created && transform.position.x > createX)
        {
            Create(transform.position.x - length);
        }
        if (created && transform.position.x > length)
        {
            Destroy(gameObject);
        }
    }

    private void Create(float x)
    {
        created = true;
        initialPos.x = x;
        Instantiate(gameObject, initialPos, Quaternion.identity);
    }
}

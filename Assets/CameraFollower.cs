using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public float zoomedInPosition;
    public float zoomedOutPosition;
    public float zoomedOutPositionFinal;
    private float _smoothFactor;

    public Transform player;

    private Director _director;

    public float followWidth = 10.0f;
    public float followHeight = 5.0f;

    private void Awake()
    {
        _director = FindObjectOfType<Director>();
    }

    private void Start()
    {
        _smoothFactor = 2;
    }

    private void Update()
    {
        if (!_director.IsGameStarted)
        {
            return;
        }

        var targetPos = new Vector3(transform.position.x, transform.position.y, zoomedOutPosition);

        if(player.position.x > transform.position.x + followWidth){
            targetPos.x = player.position.x - followWidth;
        }
        else if(player.position.x < transform.position.x - followWidth){
            targetPos.x = player.position.x + followWidth;
        }

        if(player.position.y > transform.position.y + followHeight){
            targetPos.y = player.position.y - followHeight;
        }
        else if(player.position.y < transform.position.y - followHeight){
            targetPos.y = player.position.y + followHeight;
        }

        //force zoom out on game over
        if (_director.IsGameOver)
        {
            GetComponent<GameOverCamera>().enabled = true;
            this.enabled = false;
        } 
        else
        {
            if (Input.GetKey(KeyCode.Joystick1Button4))
            {
                targetPos.z = zoomedOutPosition;
            }
            else
            {
                targetPos.z = zoomedInPosition;             
            }
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * _smoothFactor);
    }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, player.position.z), new Vector3(followBounds, followBounds, 1));
    // }
}

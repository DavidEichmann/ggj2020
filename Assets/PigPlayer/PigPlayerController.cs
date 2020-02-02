using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PigPlayerController : MonoBehaviour
{
    public float _jumpforce;
    public float _moveSpeed;
    public Vector3 _gravity = new Vector3(0, -40);
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private bool _facingRight = false;
    private float _distanceToGround;
    public static bool konamiMode = false;

    public GameObject SoundJumpO;
    public AudioSource SoundJump => SoundJumpO.GetComponent<AudioSource>();
    public GameObject SoundWalkLoopO;
    public AudioSource SoundWalkLoop => SoundWalkLoopO.GetComponent<AudioSource>();

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _distanceToGround = _collider.bounds.extents.y;

    }

    private void Update()
    {
        Physics.gravity = _gravity;
        float movement;
        {
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                movement = _moveSpeed;
            }
            else if (Input.GetAxisRaw("Horizontal") == -1)
            {
                movement = -_moveSpeed;
            }
            else
            {
                movement = 0;
            }
        }

        Move(movement);

        if (!konamiMode && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _distanceToGround * 1.05f);
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            _rigidbody.AddForce(new Vector2(0f, _jumpforce), ForceMode.Impulse);
            SoundJump.Play();
        }
    }

    void Move(float rightVelocity)
    {
        _rigidbody.velocity = new Vector3(rightVelocity, _rigidbody.velocity.y, _rigidbody.velocity.z);

        if (rightVelocity > 0 && !_facingRight)
        {
            Flip();
        }
        else if (rightVelocity < 0 && _facingRight)
        {
            Flip();
        }

        var soundWalkLoop = SoundWalkLoop;
        var isWalking = _rigidbody.velocity.x != 0 && IsGrounded();
        if (SoundWalkLoop.isPlaying && !isWalking)
        {
            soundWalkLoop.Pause();
        }
        if (!SoundWalkLoop.isPlaying && isWalking)
        {
            soundWalkLoop.Play();
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

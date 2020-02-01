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

    public UnityEvent OnJump;

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
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            Move(_moveSpeed);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            Move(-_moveSpeed);
        }
        else
        {
            Move(0);
        }

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
            OnJump.Invoke();
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
    }

    private void Flip()
    {
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

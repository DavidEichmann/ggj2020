using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigPlayerController : MonoBehaviour
{
    public float _jumpforce;
    public float _moveSpeed;
    public float _currentMove;
    public Vector3 _gravity = new Vector3(0, -40);
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private bool _facingRight = false;  
    private Vector3 _velocity = Vector3.zero;
    private float _distanceToGround;

    

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
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _currentMove = _moveSpeed;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _currentMove = -_moveSpeed;
        }
        else
        {
            _currentMove = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
       
    }

    void FixedUpdate()
    {
        Move(_currentMove);
    }

    bool IsGrounded() 
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distanceToGround + 0.1f);
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
        }
    }

    void Move(float move)
    {

        _rigidbody.velocity = new Vector3(move * 10f, _rigidbody.velocity.y);

        if (move > 0 && !_facingRight)
        {
            Flip();
        }
        else if (move < 0 && _facingRight)
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

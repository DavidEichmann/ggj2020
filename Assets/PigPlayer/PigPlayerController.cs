using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PigPlayerController : MonoBehaviour
{
    public float Jumpforce;
    public float MoveSpeed;
    // public float _runSpeed;
    public float Gravity = 40;
    public float MaxVx;
    public float MaxVy;
    public static bool KonamiMode = false;
    public float GroundFriction = 0.99f;
    public LayerMask CollisionMask;
    public float MaxJumpSlack = 1.0f;
    public float JumpDuration = 1.0f;

    public GameObject SoundJumpO;
    public AudioSource SoundJump;
    public GameObject SoundWalkLoopO;
    public AudioSource SoundWalkLoop;

    private float _vx;
    private float _vy;
    private float _ax;
    private float _ay;
    private float _jumpSlackTimer = 0.0f;
    private float _jumpTimer = 0.0f;
    private float _halfHeight;
    private float _halfWidth;

    private CapsuleCollider _collider;
    private SpriteRenderer _renderer;



    public void EnableCheatMode()
    {
        Jumpforce = 40;
        MoveSpeed = 15;
        Gravity = 3;
    }

    // Start is called before the first frame update
    void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _halfHeight = _collider.bounds.extents.y;
        _halfWidth = _collider.bounds.extents.x;
        _renderer = GetComponent<SpriteRenderer>();
        SoundJump = SoundJumpO.GetComponent<AudioSource>();
        SoundWalkLoop = SoundWalkLoopO.GetComponent<AudioSource>();
    }

    private void Update()
    {
        // if(Input.GetAxisRaw("Run") > 0.8){
        //     ax = Input.GetAxisRaw("Horizontal") * _runSpeed;
        // }
        _ax = Input.GetAxisRaw("Horizontal") * MoveSpeed;


        // Which way is pig facing?
        if (_ax > 0)
        {
            _renderer.flipX = true;
        }
        else if (_ax < 0)
        {
            _renderer.flipX = false;
        }

        // Sound Effects
        var soundWalkLoop = SoundWalkLoop;
        var isWalking = _ax != 0 && IsGrounded();
        if (SoundWalkLoop.isPlaying && !isWalking)
        {
            soundWalkLoop.Pause();
        }
        if (!SoundWalkLoop.isPlaying && isWalking)
        {
            soundWalkLoop.Play();
        }

        // Jump Stuff
        _jumpSlackTimer -= Time.deltaTime;
        if (IsGrounded())
        {
            _jumpSlackTimer = MaxJumpSlack;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (!KonamiMode && _jumpSlackTimer > 0.0)
            {
                _jumpSlackTimer = 0.0f;
                _jumpTimer = JumpDuration;
                SoundJump.Play();
            }
        }
        else if (!Input.GetButton("Jump"))
        {
            _jumpTimer = 0.0f;
        }
    }

    void FixedUpdate()
    {

        // Pre-Collision Movement
        _vx = (_vx + _ax) * GroundFriction;
        _vx = Mathf.Clamp(_vx, -MaxVx, MaxVx);
        transform.Translate(_vx * Time.fixedDeltaTime, 0.0f, 0.0f);

        RaycastHit horHit;
        var numRays = 9;
        // var vRayYs = new List<float>{transform.position.y - _halfHeight, transform.position.y, transform.position.y + _halfHeight};
        List<Vector3> rayOriginsH = Enumerable.Range(0, numRays)
        .Select(y => 2 * _halfHeight * y / (numRays - 1) - _halfHeight)
        .Select(y => new Vector3(transform.position.x, transform.position.y + y, transform.position.z))
        .ToList();

        if (_vx < 0)
        {
            foreach (var origin in rayOriginsH)
            {
                if (Physics.Raycast(origin, Vector3.left, out horHit, _halfWidth * 1.05f, CollisionMask))
                {
                    transform.position = new Vector3(horHit.point.x + _halfWidth * 1.05f, transform.position.y, transform.position.z);
                    _vx = 0;
                }
            }
        }
        if (_vx > 0)
        {
            foreach (var origin in rayOriginsH)
            {
                if (Physics.Raycast(origin, Vector3.right, out horHit, _halfWidth * 1.05f, CollisionMask))
                {
                    transform.position = new Vector3(horHit.point.x - _halfWidth * 1.05f, transform.position.y, transform.position.z);
                    _vx = 0;
                }
            }
        }

        _jumpTimer -= Time.fixedDeltaTime;
        if (_jumpTimer > 0)
        {
            _vy = Jumpforce;
        }
        else
        {
            _vy -= Gravity;
        }

        _vy = Mathf.Clamp(_vy, -MaxVy, MaxVy);
        transform.Translate(0.0f, _vy * Time.fixedDeltaTime, 0.0f);

        // Vertical Collision Detection
        RaycastHit vertHit;
        var vRayXs = new List<float> { transform.position.x - _halfWidth, transform.position.x, transform.position.x + _halfWidth };
        if (_vy < 0)
        {
            foreach (var x in vRayXs)
            {
                if (Physics.Raycast(new Vector3(x, transform.position.y, transform.position.z), Vector3.down, out vertHit, _halfHeight, CollisionMask))
                {
                    transform.position = new Vector3(transform.position.x, vertHit.point.y + _halfHeight, transform.position.z);
                    Debug.DrawRay(new Vector3(x, transform.position.y, transform.position.z), Vector3.down, Color.red);
                    _vy = 0;
                }
            }
        }
        if (_vy > 0)
        {
            foreach (var x in vRayXs)
            {
                if (Physics.Raycast(new Vector3(x, transform.position.y, transform.position.z), Vector3.up, out vertHit, _halfHeight, CollisionMask))
                {
                    Debug.DrawRay(new Vector3(x, transform.position.y, transform.position.z), Vector3.up, Color.red, 1.0f);
                    transform.position = new Vector3(transform.position.x, vertHit.point.y - _halfHeight, transform.position.z);
                    _jumpTimer = 0;
                    _vy = 0;
                }
            }
        }
    }

    bool IsGrounded()
    {
        var vRayXs = new List<float> { transform.position.x - _halfWidth, transform.position.x, transform.position.x + _halfWidth };
        foreach (var x in vRayXs)
        {
            if (Physics.Raycast(new Vector3(x, transform.position.y, transform.position.z), Vector3.down, _halfHeight * 1.05f, CollisionMask) && _vy <= 0)
            {
                return true;
            }
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }
}

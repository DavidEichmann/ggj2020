using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PigPlayerController : MonoBehaviour
{
    public float _jumpforce;
    public float _moveSpeed;
    // public float _runSpeed;
    public float _gravity = 40;
    public float _maxVx;
    public float _maxVy;
    private float vx;
    private float vy;
    private float ax;
    private float ay;
    public float groundFriction = 0.99f;
    private CapsuleCollider _collider;
    public LayerMask collisionMask;
    private SpriteRenderer _renderer;
    private float _halfHeight;
    private float _halfWidth;
    public static bool konamiMode = false;

    public GameObject SoundJumpO;
    public AudioSource SoundJump => SoundJumpO.GetComponent<AudioSource>();
    public GameObject SoundWalkLoopO;
    public AudioSource SoundWalkLoop => SoundWalkLoopO.GetComponent<AudioSource>();

    private float jumpSlackTimer = 0.0f;
    public float _maxJumpSlack = 1.0f;
    private float jumpTimer = 0.0f;
    public float _jumpDuration = 1.0f;

    // Start is called before the first frame update
    void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _halfHeight = _collider.bounds.extents.y;
        _halfWidth = _collider.bounds.extents.x;
        _renderer = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        // if(Input.GetAxisRaw("Run") > 0.8){
        //     ax = Input.GetAxisRaw("Horizontal") * _runSpeed;
        // }
        ax = Input.GetAxisRaw("Horizontal") * _moveSpeed;


        // Which way is pig facing?
        if (ax > 0)
        {
            _renderer.flipX = true;
        }
        else if (ax < 0)
        {
            _renderer.flipX = false;
        }

        // Sound Effects
        var soundWalkLoop = SoundWalkLoop;
        var isWalking = ax != 0 && IsGrounded();
        if (SoundWalkLoop.isPlaying && !isWalking)
        {
            soundWalkLoop.Pause();
        }
        if (!SoundWalkLoop.isPlaying && isWalking)
        {
            soundWalkLoop.Play();
        }

        // Jump Stuff
        jumpSlackTimer -= Time.deltaTime;
        if(IsGrounded()){
            jumpSlackTimer = _maxJumpSlack;
        }

        if(Input.GetButtonDown("Jump")){
            if (!konamiMode && jumpSlackTimer > 0.0)
            {
                jumpSlackTimer = 0.0f;
                jumpTimer = _jumpDuration;
                SoundJump.Play();
            }
        }
        else if(!Input.GetButton("Jump")){
            jumpTimer = 0.0f;
        }
    }

    void ResolveCollisions(Vector3 direction, float distToEdge){
        return;
    }

    void FixedUpdate(){

        // Pre-Collision Movement
        vx = (vx + ax) * groundFriction;
        vx = Mathf.Clamp(vx, -_maxVx, _maxVx);
        transform.Translate(vx * Time.fixedDeltaTime, 0.0f, 0.0f);

        RaycastHit horHit;
        var numRays = 9;
        // var vRayYs = new List<float>{transform.position.y - _halfHeight, transform.position.y, transform.position.y + _halfHeight};
        List<Vector3> rayOriginsH = Enumerable.Range(0, numRays)
        .Select(y => 2 * _halfHeight * y / (numRays - 1) - _halfHeight)
        .Select(y => new Vector3(transform.position.x, transform.position.y + y, transform.position.z))
        .ToList();

        if(vx < 0){
            foreach(var origin in rayOriginsH){
                if (Physics.Raycast(origin, Vector3.left, out horHit, _halfWidth * 1.05f, collisionMask))
                {
                    transform.position = new Vector3(horHit.point.x + _halfWidth * 1.05f, transform.position.y, transform.position.z);
                    vx = 0;
                }
            }
        }
        if(vx > 0){
            foreach(var origin in rayOriginsH){
                if (Physics.Raycast(origin, Vector3.right, out horHit, _halfWidth * 1.05f, collisionMask))
                {
                    transform.position = new Vector3(horHit.point.x - _halfWidth * 1.05f, transform.position.y, transform.position.z);
                    vx = 0;
                }
            }
        }


        jumpTimer -= Time.fixedDeltaTime;
        if(jumpTimer > 0){
            vy = _jumpforce;
        }
        else{
            vy -= _gravity;
        }

        vy = Mathf.Clamp(vy, -_maxVy, _maxVy);
        transform.Translate(0.0f, vy * Time.fixedDeltaTime, 0.0f);

        // Vertical Collision Detection
        RaycastHit vertHit;
        var vRayXs = new List<float>{transform.position.x - _halfWidth, transform.position.x, transform.position.x + _halfWidth};
        if(vy < 0){
            foreach(var x in vRayXs){
                if (Physics.Raycast(new Vector3(x, transform.position.y, transform.position.z), Vector3.down, out vertHit, _halfHeight, collisionMask))
                {
                    transform.position = new Vector3(transform.position.x, vertHit.point.y + _halfHeight, transform.position.z);
                    Debug.DrawRay(new Vector3(x, transform.position.y, transform.position.z), Vector3.down, Color.red);
                    vy = 0;
                }
            }
        }
        if(vy > 0){
            foreach(var x in vRayXs){
                if (Physics.Raycast(new Vector3(x, transform.position.y, transform.position.z), Vector3.up, out vertHit, _halfHeight, collisionMask))
                {
                    Debug.DrawRay(new Vector3(x, transform.position.y, transform.position.z), Vector3.up, Color.red, 1.0f);
                    transform.position = new Vector3(transform.position.x, vertHit.point.y - _halfHeight, transform.position.z);
                    jumpTimer = 0;
                    vy = 0;
                }
            }
        }
    }

    bool IsGrounded()
    {
        var vRayXs = new List<float>{transform.position.x - _halfWidth, transform.position.x, transform.position.x + _halfWidth};
        foreach(var x in vRayXs){
            if(Physics.Raycast(new Vector3(x, transform.position.y, transform.position.z), Vector3.down, _halfHeight * 1.05f, collisionMask) && vy <= 0){
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

    void Jump()
    {
    }

    void Move(float moveVel)
    {

    }
}

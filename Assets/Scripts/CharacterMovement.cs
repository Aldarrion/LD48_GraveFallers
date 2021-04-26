using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float Speed = 5;

    public float GroundFriction;
    public float AirFriction;
    public float MovementSpeed;
    public float MaxVerticalSpeed;

    public float JumpTime;
    public float JumpHeight;
    public float FallGravityMultiplier;
    public MovementComponent MovementComponent { get; private set; }

    public string Prefix;

    private Rigidbody2D _rigidBody;
    private bool _isGrounded;
    private bool _jumpKeyWasUp = true;


    private float DefaultGravityScale => (2 * GetJumpHeight() / Mathf.Pow(GetJumpTime(), 2)) / 9.89f;
    //public float DefaultGravityScale = 1.2232f;
    public float FallingGravityScale => FallGravityMultiplier * DefaultGravityScale;

    private float InitialVelocity => 2 * GetJumpHeight() / GetJumpTime();
    private float GetJumpHeight()
    {
        return JumpHeight;
    }

    private float GetJumpTime()
    {
        return GetJumpHeight() * JumpTime / JumpHeight;
    }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        MovementComponent = new MovementComponent(transform, GetComponent<Collider2D>(), Speed, Prefix);
    }

    void Update()
    {
        MovementComponent.Update();

        transform.position = new Vector3(Mathf.Max(Mathf.Min(transform.position.x, 6), -6), transform.position.y, transform.position.z);
    }

    private void OnDisable()
    {
        if (MovementComponent != null)
            MovementComponent.Disable();
    }

    private void OnEnable()
    {
        if (MovementComponent != null)
            MovementComponent.Enable();
    }

    void FixedUpdate()
    {
        //Move();
    }

    public void Move()
    {
        Vector3 velocity = _rigidBody.velocity;

        bool isLeftKeyActive = Input.GetAxis(Prefix + "Horizontal") < 0;
        bool isRightKeyActive = Input.GetAxis(Prefix + "Horizontal") > 0;
        bool isJumpKeyActive = Input.GetButton(Prefix + "Jump");
        bool directionKeyIsActive = false;
        // -----------------------
        // Horiznotal movement
        if (isLeftKeyActive ^ isRightKeyActive)
        {
            if (isRightKeyActive)
            {
                directionKeyIsActive = true;
                if (velocity.x < 0.0f)
                {
                    velocity.x *= GroundFriction;
                }
                velocity.x += Time.deltaTime * MovementSpeed;
            }
            if (isLeftKeyActive)
            {
                directionKeyIsActive = true;
                if (velocity.x > 0.0f)
                {
                    velocity.x *= GroundFriction;
                }
                velocity.x -= Time.deltaTime * MovementSpeed;
            }
        }

        if (!directionKeyIsActive)
        {
            velocity.x *= _isGrounded ? GroundFriction : AirFriction;
        }

        velocity.x = Mathf.Clamp(velocity.x, -MaxVerticalSpeed, MaxVerticalSpeed);

        // -----------------------
        // Vertical movement
        if (isJumpKeyActive && _isGrounded && _jumpKeyWasUp)
        {
            velocity.y = InitialVelocity;
            //_animator.Play("Hero_JumpStart");
            _jumpKeyWasUp = false;
        }

        if (_isGrounded && !isJumpKeyActive)
            _jumpKeyWasUp = true;

        if (velocity.y < 0)
        {
            _rigidBody.gravityScale = FallingGravityScale;
        }
        else
        {
            _rigidBody.gravityScale = DefaultGravityScale;
        }

        velocity.y = Mathf.Max(velocity.y, -30);
        _rigidBody.velocity = velocity;

        // -----------------------
        // Animations and effects
        //MovementEffects();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            _isGrounded = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            _isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            _isGrounded = true;
    }
}

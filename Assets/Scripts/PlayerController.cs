using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform playerTransform;
    public ShieldController shieldController;
    
    [Header("Move")] 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private Transform groundCheckPosition;
    private InputAction _moveAction;
    
    [Header("Jump")]
    [SerializeField] private float jumpPower = 2.5f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private Animator animator;
    [SerializeField] private int maxDoubleJump = 2;
    private InputAction _jumpAction;
    
    private float _jumpBufferTimer;
    private bool _isGrounded;
    private bool _isInvincible;
    private Vector2 _velocity;
    private int _doubleJumpCount;
    
    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void OnEnable()
    {
        _jumpBufferTimer = 0;
        _doubleJumpCount = 0;
        _velocity = Vector2.zero;
    }

    private void Update()
    {
        HandleTimers(Time.deltaTime);
        UpdateMove(Time.deltaTime);
    }
    
    private void FixedUpdate()
    {
        CheckIsGrounded();
    }
    
    private void CheckIsGrounded()
    {
        _isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
        if (_isGrounded && rb.linearVelocityY <= 0)
        {
            _doubleJumpCount = 0;
        }    
    }
    
    private void HandleTimers(float deltaTime)
    {
        if (_jumpAction.WasPressedThisFrame())
        {
            _jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            _jumpBufferTimer -= deltaTime;
        }
    }
    
    private void UpdateMove(float deltaTime)
    {
        var move = _moveAction.ReadValue<Vector2>();
        
        _velocity.x = move.x * moveSpeed;

        if (_jumpBufferTimer > 0)
        {
            if (_isGrounded || (!_isGrounded && _doubleJumpCount < maxDoubleJump))
            {
                Jump();
            }
        }
        
        rb.linearVelocity = new Vector2(_velocity.x, rb.linearVelocity.y);

        if (move.magnitude > 0)
        {
            playerTransform.localScale = new Vector3(move.x > 0 ? 1 : -1, 1, 1);
        }

        //animator.SetFloat(MoveSpeedAnimatorFloat, _currentMoveVelocity.magnitude / maxMoveSpeed);
    }

    private void Jump()
    {
        CheckIsGrounded();
        
        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        
        _jumpBufferTimer = 0;
        _doubleJumpCount++;
    }
    
    public bool IsInvincible
    {
        get => _isInvincible;
        set => OnSetInvincible(value);
    }
    
    private void OnSetInvincible(bool value)
    {
        _isInvincible = value;
        // TODO: Implement invincibility effect
    }
}
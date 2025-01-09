using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform playerTransform;
    public RingController ringController;
    
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

    [Header("Dash")] 
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private InputAction _dashAction;
    
    private float _jumpBufferTimer;
    private bool _isGrounded;
    private bool _isInvincible;
    private Vector2 _velocity;
    private int _doubleJumpCount;
    private float _dashCooldownTimer;
    
    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _dashAction = InputSystem.actions.FindAction("Dash");
    }

    private void OnEnable()
    {
        _jumpBufferTimer = 0;
        _doubleJumpCount = 0;
        _velocity = Vector2.zero;
    }

    private void Update()
    {
        HandleTimers();
        
        UpdateMove();
        
        if (_jumpBufferTimer > 0)
        {
            if (_isGrounded || (!_isGrounded && _doubleJumpCount < maxDoubleJump))
            {
                Jump();
            }
        }
        
        if (_dashAction.WasPressedThisFrame())
        {
            StartCoroutine(Dash());
        }
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
    
    private void HandleTimers()
    {
        if (_jumpAction.WasPressedThisFrame())
        {
            _jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            _jumpBufferTimer -= Time.deltaTime;
        }
        
        if (_dashCooldownTimer > 0)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }
        else if (_dashCooldownTimer < 0)
        {
            _dashCooldownTimer = 0;
        }
    }
    
    private void UpdateMove()
    {
        var move = _moveAction.ReadValue<Vector2>();
        
        _velocity.x = move.x * moveSpeed;
        
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

    private IEnumerator Dash()
    {
        if (_dashCooldownTimer > 0)
        {
            yield break;
        }
        _dashCooldownTimer = dashCooldown;

        var y = _moveAction.ReadValue<Vector2>().y > 0 ? 1 : 0;
        var x = playerTransform.localScale.x > 0 ? 1 : -1;
        var direction = new Vector2(x, y).normalized;
        rb.DOMove(rb.position + direction * dashDistance, dashDuration);
        
        IsInvincible = true;
        rb.linearVelocityY = 0;
        
        yield return new WaitForSeconds(dashDuration);
        
        IsInvincible = false;
    }
    
    public bool IsInvincible
    {
        get => _isInvincible;
        set => OnSetInvincible(value);
    }
    
    private void OnSetInvincible(bool value)
    {
        _isInvincible = value;
    }
}
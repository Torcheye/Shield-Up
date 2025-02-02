using System.Collections;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private RingController ringController;
    
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
    
    [Header("Rendering")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Color trailColor;
    [SerializeField] private Color jumpTrailColor;
    [SerializeField] private Color dashTrailColor;
    [SerializeField] private float jumpTrailTime;
    
    private float _jumpBufferTimer;
    private bool _isGrounded;
    private bool _isInvincible;
    private Vector2 _velocity;
    private int _doubleJumpCount;
    private float _dashCooldownTimer;
    private float _slowMultiplier;
    private float _dizzyMultiplier;
    private Tween _dashTween;
    
    private static readonly int JumpTrigger = Animator.StringToHash("Trigger Jump");
    private static readonly int DashTrigger = Animator.StringToHash("Trigger Dash");
    private static readonly int IsRunningParam = Animator.StringToHash("Running");
    
    public void Teleport(Vector2 position)
    {
        transform.position = position;
        ringController.transform.position = position;
        
        trail.Clear();
        ringController.DisableAllWeaponTrails();
        
        if (_dashTween != null && _dashTween.IsActive())
        {
            _dashTween.Kill();
        }
    }
    
    public float GetVelocityX() => _velocity.x;
    
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
        _dashCooldownTimer = 0;
        _slowMultiplier = 1;
        _dizzyMultiplier = 1;
        trail.startColor = trailColor;
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheckPosition.position, groundCheckPosition.position + Vector3.down * groundCheckDistance);
    }
    
#endif
    
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
        
        _velocity.x = move.x * moveSpeed * _slowMultiplier * _dizzyMultiplier;
        
        rb.linearVelocity = new Vector2(_velocity.x, rb.linearVelocity.y);

        var isMoving = Mathf.Abs(_velocity.x) > 0;

        if (isMoving)
        {
            playerTransform.localScale = new Vector3(move.x > 0 ? 1 : -1, 1, 1);
        }

        animator.SetBool(IsRunningParam, isMoving);
    }

    private void Jump()
    {
        CheckIsGrounded();

        animator.SetTrigger(JumpTrigger);

        rb.linearVelocityY = 0;
        rb.AddForce(_slowMultiplier * jumpPower * Vector2.up, ForceMode2D.Impulse);
        
        _jumpBufferTimer = 0;
        _doubleJumpCount++;
        StartCoroutine(JumpTrail());
    }
    
    private IEnumerator JumpTrail()
    {
        trail.startColor = jumpTrailColor;
        yield return new WaitForSeconds(jumpTrailTime);
        trail.startColor = trailColor;
    }

    private IEnumerator Dash()
    {
        if (_dashCooldownTimer > 0)
        {
            yield break;
        }
        _dashCooldownTimer = dashCooldown;


        animator.SetTrigger(DashTrigger);
        spriteRenderer.material.EnableKeyword("HSV_ON");
        spriteRenderer.material.EnableKeyword("CHROMABERR_ON");
        trail.startColor = dashTrailColor;

        var y = _moveAction.ReadValue<Vector2>().y > 0 ? 1 : 0;
        var x = playerTransform.localScale.x > 0 ? 1 : -1;
        var direction = new Vector2(x, y).normalized;
        _dashTween = rb.DOMove(rb.position + _slowMultiplier * dashDistance * direction, dashDuration);
        
        statusEffect.ApplyEffect(Effect.Invulnerable, dashDuration);
        rb.linearVelocityY = 0;
        
        yield return new WaitForSeconds(dashDuration);
        
        spriteRenderer.material.DisableKeyword("HSV_ON");
        spriteRenderer.material.DisableKeyword("CHROMABERR_ON");
        trail.startColor = trailColor;
    }
    
    public void SetSlowMultiplier(bool on)
    {
        _slowMultiplier = on ? DataManager.Instance.slowEffectMultiplier : 1;
    }
    
    public void SetDizzyMultiplier(bool on)
    {
        _dizzyMultiplier = on ? -1 : 1;
    }
}
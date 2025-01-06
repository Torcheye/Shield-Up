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
    private InputAction _jumpAction;
    
    private float _jumpBufferTimer;
    private bool _isGrounded;
    
    private bool _isInvincible;
    private Vector2 _velocity;
    
    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
    }
    
    private void Update()
    {
        HandleTimers(Time.deltaTime);
        UpdateMove(Time.deltaTime);
    }
    
    private void FixedUpdate()
    {
        _isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
        Debug.DrawRay(groundCheckPosition.position, Vector2.down * groundCheckDistance, Color.red);
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

        if (_jumpBufferTimer > 0 && _isGrounded)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            //animator.SetTrigger(JumpStartTrigger);
            _jumpBufferTimer = 0;
        }
        
        rb.linearVelocity = new Vector2(_velocity.x, rb.linearVelocity.y);

        if (move.magnitude > 0)
        {
            playerTransform.localScale = new Vector3(move.x > 0 ? 1 : -1, 1, 1);
        }

        //animator.SetFloat(MoveSpeedAnimatorFloat, _currentMoveVelocity.magnitude / maxMoveSpeed);
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
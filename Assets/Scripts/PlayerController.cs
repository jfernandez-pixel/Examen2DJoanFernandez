using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Rigidbody2D _rBody;
    private Vector2 _moveInput;
    private Animator _animator;

    [SerializeField]private Transform _groundSensor;
    [SerializeField]private float _jumpForce = 5f;
    [SerializeField]private float _movementSpeed = 6f;
    [SerializeField]private float _sensorRadius = 0.5f;

    void Awake()
    {
        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];

        _rBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();

        if(_moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsRunning", true);
        }
        else if(_moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }

        if(_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }
        _animator.SetBool("IsJumping", !IsGrounded());
    }

    void FixedUpdate()
    {
        _rBody.linearVelocityX = _moveInput.x * _movementSpeed;
    }

    void Jump()
    {
        _rBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    bool IsGrounded()
    {
        Collider2D[]colliders = Physics2D.OverlapCircleAll(_groundSensor.position, _sensorRadius);
        foreach(Collider2D item in colliders)
        {
            if(item.gameObject.layer == 6)
            {
                return true;
            }
        }
        return false;
    }
}

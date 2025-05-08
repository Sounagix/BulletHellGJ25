using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    public static event System.Action OnPlayerMouseLeftClickedDown;
    public static event System.Action OnPlayerMouseLeftClickedUp;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private MovementStats _movementStats;

    [SerializeField]
    private float _dashForce = 20f;

    private bool _isDashing = false;
    private bool _dashRequested = false;
    private Vector2 _dashDirection;
    private PlayerManager _playerManager;

    #region Input Callbacks

    public void OnMove(CallbackContext value)
    {
        if (!_playerManager)
            return;

        _movementStats.MovementDir = value.ReadValue<Vector2>();
    }

    public void OnMouseLeftDown(CallbackContext value)
    {
        if (!_playerManager)
            return;

        if (value.phase == InputActionPhase.Performed)
            OnPlayerMouseLeftClickedDown?.Invoke();

        if (value.phase == InputActionPhase.Canceled)
            OnPlayerMouseLeftClickedUp?.Invoke();
    }

    public void OnSprint()
    {
        if (_isDashing || _dashRequested || !_playerManager)
            return;

        _dashRequested = true;
        _isDashing = true;
        _dashDirection = _movementStats.MovementDir.normalized;
    }

    #endregion

    public void SetUp(PlayerManager playerManager)
    {
        _playerManager = playerManager;
        _movementStats.CurrentMaxSpeed = _movementStats.MaxSpeed;
    }

    private void FixedUpdate()
    {
        if (!_rb || !_playerManager)
            return;

        HandleMovementForce();
    }

    private void HandleMovementForce()
    {
        _movementStats.CurrentVelocity = _rb.linearVelocity;
        _rb.AddForce(_movementStats.MovementDir * _movementStats.MovementForce, ForceMode2D.Force);

        if (_rb.linearVelocity.magnitude > _movementStats.CurrentMaxSpeed)
            _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, _movementStats.CurrentMaxSpeed);
    }

    public Vector2 GetVelocity()
    {
        return _rb.linearVelocity;
    }

    // TODO: Dash
    private void HandleDashForce()
    {
        if(_isDashing && _rb.linearVelocity.magnitude <= _movementStats.CurrentMaxSpeed + 0.1f) 
        {
            _isDashing = false;
        }
        else if (_dashRequested) 
        {
            _rb.AddForce(_dashDirection.normalized * _dashForce, ForceMode2D.Impulse);
            _dashRequested = false;
        }
    }
}

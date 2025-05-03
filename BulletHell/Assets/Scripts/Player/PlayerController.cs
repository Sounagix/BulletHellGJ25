using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;

    [Header("Movement Factors")]
    [SerializeField]
    private float _movementForce = 1;

    [SerializeField] 
    private float _maxSpeed = 5;

    private Vector2 _movementDir;
    private Vector2 _currentVelocity;
    // In case we want to slow down the player, we need this
    private float _currentMaxSpeed = 0;

    #region Input Callbacks

    public void OnMove(InputValue value)
    {
        _movementDir = value.Get<Vector2>();
    }

    #endregion

    public void SetUp(PlayerManager playerManager)
    {
        _currentMaxSpeed = _maxSpeed;
    }

    private void FixedUpdate()
    {
        if (!_rb)
            return;

        _currentVelocity = _rb.linearVelocity;

        _rb.AddForce(_movementDir * _movementForce, ForceMode2D.Force);

        if (_rb.linearVelocityX > _currentMaxSpeed)
            _currentVelocity.x = _currentMaxSpeed;
        if (_rb.linearVelocityY > _currentMaxSpeed)
            _currentVelocity.y = _currentMaxSpeed;

        _rb.linearVelocity = _currentVelocity;
    }
}

using System.Collections;
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
    private float _dashCooldown;

    [SerializeField]
    private float _dashDuration;

    [SerializeField]
    private float _dashDistance;

    [SerializeField]
    private TrailRenderer _trailRenderer;

    private Coroutine _dashCoroutine;

    [SerializeField]
    protected LayerMask _borderLayer;

    private bool _isDashing = false;
    private bool _dashRequested = false;
    private PlayerManager _playerManager;

    #region Input Callbacks

    public void OnMove(CallbackContext value)
    {
        if (!_playerManager)
            return;

        _movementStats.MovementDir = value.ReadValue<Vector2>();
        TutorialManager.OnTutorialUpdate?.Invoke(TUTORIAL.MOVEMENT);
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
    }

    #endregion

    public void SetUp(PlayerManager playerManager)
    {
        _playerManager = playerManager;
        _movementStats.CurrentMaxSpeed = _movementStats.MaxSpeed;
        _trailRenderer.emitting = false;
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

    public void HandleDash(CallbackContext value)
    {
        if (_dashCoroutine == null)
        {
            _dashCoroutine = StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        MasterAudioManager.Instance.PlayOneShot(PLAYER_SOUNDS.DASH, transform);
        _trailRenderer.emitting = true;
        Vector2 startPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDir = _movementStats.MovementDir.normalized;
        Vector2 targetPos = startPos + dashDir * _dashDistance;

        float elapsed = 0f;

        while (elapsed < _dashDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _dashDuration;
            transform.position = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;

        yield return new WaitForSeconds(_dashCooldown);
        _dashCoroutine = null;
        _trailRenderer.emitting = false;
        TutorialManager.OnTutorialUpdate?.Invoke(TUTORIAL.DASH);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int mask = 1 << collision.gameObject.layer;

        if ((mask & _borderLayer.value) != 0)
        {
            Vector2 normal = (transform.position - collision.transform.position).normalized;
            Vector2 dir = -_movementStats.MovementDir;
            dir *= 20;

            _rb.AddForce(dir, ForceMode2D.Impulse);

            PTCManager.OnEventPTCCreate?.Invoke(PTCType.BorderCollision, transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        int mask = 1 << collision.gameObject.layer;

        if ((mask & _borderLayer.value) != 0)
        {
            Vector2 dir = (Vector2.zero - (Vector2)transform.position).normalized;
            dir *= 20;

            _rb.AddForce(dir, ForceMode2D.Impulse);

            PTCManager.OnEventPTCCreate?.Invoke(PTCType.BorderCollision, transform.position);
        }
    }
}

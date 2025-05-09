using System;
using UnityEngine;

public abstract class InteractableController : MonoBehaviour
{
    public static event Action<InteractableController> OnBackToThePool;
    public static event Action<Transform> OnInteractableChange;

    [SerializeField]
    private InteractableType _interactableType;
    public InteractableType InteractableType { get { return _interactableType; } }

    [SerializeField]
    [Tooltip("[Min, Max]")]
    private RangeFloat _lifeTime;

    [SerializeField]
    protected Rigidbody2D _rb;

    [SerializeField]
    protected MovementStats _movementStats;

    [SerializeField]
    protected LayerMask _borderLayer;

    [SerializeField]
    LayerMask _playerLayer;

    [SerializeField]
    protected Gradient _trailColor;

    [SerializeField]
    private TrailRenderer _trailRenderer;

    public MovementStats MovementStats { get { return _movementStats; } set { _movementStats = value; } }

    private float _currentLifeTime;
    private float _currentTime = 0;
    private Quaternion _originalRotation;
    private Vector3 _originalScale;
    private bool _isChangeable = false;
    private float _timeToChangeRange;

    protected bool isActive = false;

    public virtual void SetUp()
    {
        _currentLifeTime = UnityEngine.Random.Range(_lifeTime.Min, _lifeTime.Max);
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
        _trailRenderer.colorGradient = _trailColor;
    }

    protected virtual void ReturnToThePool(bool changeObject = false)
    {
        if (changeObject)
        {
            OnInteractableChange?.Invoke(transform);
        }

        // Back to the pool
        isActive = false;
        OnBackToThePool?.Invoke(this);
    }

    #region Unity Callbacks

    public virtual void Update()
    {
        _currentTime += Time.deltaTime;
        if (_isChangeable && _currentLifeTime >= _timeToChangeRange)
        {
            ReturnToThePool(changeObject: true);
            return;
        }

        if (_currentTime < _currentLifeTime || !isActive)
            return;

        // Return to the pool
        ReturnToThePool();
    }

    protected virtual void FixedUpdate()
    {
        if (!_rb && !isActive)
            return;

        Vector2 dir = _movementStats.MovementDir;
        _rb.linearVelocity = _movementStats.MovementForce * _movementStats.MovementDir;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        int mask = 1 << collision.gameObject.layer;
        if ((mask & _playerLayer) != 0)
        {
            OnPlayerTouched();
            ReturnToThePool();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        int mask = 1 << collision.gameObject.layer;

        if ((mask & _borderLayer.value) != 0)
        {
            Vector2 normal = collision.GetContact(0).normal;
            _movementStats.MovementDir = Vector2.Reflect(_movementStats.MovementDir, normal).normalized;
            PTCManager.OnEventPTCCreate?.Invoke(PTCType.BorderCollision, transform.position);
            MasterAudioManager.Instance.PlayOneShot(THROWEABLE_SOUND.BOUNCE, transform);
        }
        else if ((mask & _playerLayer) != 0)
        {
            OnPlayerTouched();
            ReturnToThePool();
        }
    }

    #endregion

    #region Public

    protected void ResetObject(Vector2 spawnPoint, bool wasChangeable)
    {
        // Life time and Change Time
        _currentLifeTime = UnityEngine.Random.Range(_lifeTime.Min, _lifeTime.Max);
        _isChangeable = !wasChangeable && UnityEngine.Random.value > 0.8f;
        _timeToChangeRange = UnityEngine.Random.Range(_lifeTime.Min, _currentLifeTime);
        // Transform reset
        transform.position = spawnPoint;
        transform.rotation = _originalRotation;
        transform.localScale = _originalScale;
        // Others
        _currentTime = 0;
        isActive = true;
    }

    public virtual void UpdateTargetPosition(Vector2 target)
    {
        _movementStats.MovementDir = target - (Vector2)transform.position;
    }

    public virtual void UpdateProjectileForce(float newForce)
    {
        _movementStats.MovementForce = newForce;
    }

    #endregion

    protected abstract void OnPlayerTouched();
}

using System;
using System.Collections;
using UnityEngine;

public abstract class InteractableController : MonoBehaviour
{
    public static event Action<InteractableController> OnBackToThePool;
    public static event Action<Transform> OnInteractableChange;

    [SerializeField]
    private InteractableType _interactableType;
    public InteractableType InteractableType { get { return _interactableType; } }

    [Header("Attributes")]
    [SerializeField]
    [Tooltip("[Min, Max]")]
    private RangeFloat _lifeTime;

    [SerializeField]
    protected Rigidbody2D _rb;

    [SerializeField]
    protected MovementStats _movementStats;

    [Header("Layers to hanlde")]
    [SerializeField]
    protected LayerMask _borderLayer;

    [SerializeField]
    LayerMask _playerLayer;

    [Header("Effects")]
    [SerializeField]
    protected Gradient _trailColor;

    [SerializeField]
    private TrailRenderer _trailRenderer;

    [SerializeField]
    private SpriteRenderer _glitchRenderer;

    [SerializeField]
    private float _glitchFadeOutDuration;

    public MovementStats MovementStats { get { return _movementStats; } set { _movementStats = value; } }

    private float _currentLifeTime;
    private float _currentTime = 0;
    private Quaternion _originalRotation;
    private Vector3 _originalScale;
    private bool _isChangeable = false;
    private float _timeToChangeRange;
    private Color _glitchOriginalColor;

    protected bool isActive = false;

    public virtual void SetUp()
    {
        _currentLifeTime = UnityEngine.Random.Range(_lifeTime.Min, _lifeTime.Max);
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
        _trailRenderer.colorGradient = _trailColor;
        _glitchOriginalColor = _glitchRenderer.color;
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
        if (_isChangeable && _currentTime >= _timeToChangeRange)
        {
            ReturnToThePool(changeObject: true);
            return;
        }

        if (_currentTime < _currentLifeTime || !isActive)
            return;

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
        _isChangeable = !wasChangeable && UnityEngine.Random.value > 0.6f;
        if (_isChangeable)
            _timeToChangeRange = UnityEngine.Random.Range(_lifeTime.Min, _currentLifeTime);

        // Transform reset
        transform.position = spawnPoint;
        transform.rotation = _originalRotation;
        transform.localScale = _originalScale;
        // Others
        _currentTime = 0;
        isActive = true;
        // Glitch Renderer
        _glitchRenderer.gameObject.SetActive(false);
        _glitchRenderer.color = _glitchOriginalColor;
    }

    public virtual void UpdateTargetPosition(Vector2 target)
    {
        _movementStats.MovementDir = target - (Vector2)transform.position;
    }

    public virtual void UpdateProjectileForce(float newForce)
    {
        _movementStats.MovementForce = newForce;
    }

    public void EnableGlitchEffect()
    {
        _glitchRenderer.gameObject.SetActive(true);
        StartCoroutine(GlitchFadeOut());
    }

    private IEnumerator GlitchFadeOut()
    {
        yield return new WaitForSeconds(_glitchFadeOutDuration);
        _glitchRenderer.gameObject.SetActive(false);
    }


    #endregion

    protected abstract void OnPlayerTouched();
}

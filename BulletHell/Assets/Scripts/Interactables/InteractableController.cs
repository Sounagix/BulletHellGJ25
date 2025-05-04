using System;
using UnityEngine;

[Serializable]
public struct LifeTimeRange
{
    public float MinLifeTimeSec;
    public float MaxLifeTimeSec;
}

public class InteractableController : MonoBehaviour
{
    public static event Action<InteractableController> OnBackToThePool;

    [SerializeField]
    private InteractableType _interactableType;
    public InteractableType InteractableType { get { return _interactableType; } }

    [SerializeField]
    [Tooltip("[Min, Max]")]
    private LifeTimeRange _lifeTime;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private MovementStats _movementStats;

    [SerializeField] 
    LayerMask _borderLayer;
    
    [SerializeField] 
    LayerMask _playerLayer;

    public MovementStats MovementStats { get { return _movementStats; } }

    private float _currentLifeTime;
    private float _currentTime = 0;
    private bool _isActive = false;

    public virtual void SetUp() 
    {
        _currentLifeTime = UnityEngine.Random.Range(_lifeTime.MinLifeTimeSec, _lifeTime.MaxLifeTimeSec);
    }

    public virtual void OnInteract()
    {
        // Back to the pool
        _isActive = false;
        OnBackToThePool?.Invoke(this);
    }

    #region Unity Callbacks

    public virtual void Update() 
    {
        _currentTime += Time.deltaTime;
        if (_currentTime < _currentLifeTime || !_isActive)
            return;

        // Return to the pool
        _isActive = false;
        OnBackToThePool?.Invoke(this);
    }

    private void FixedUpdate()
    {
        if (!_rb && !_isActive)
            return;

        Vector2 dir = _movementStats.MovementDir;
        _rb.linearVelocity = _movementStats.MovementForce * _movementStats.MovementDir;
    }

    #endregion
    public virtual void ResetObject(Vector2 spawnPoint) 
    {
        _currentLifeTime = UnityEngine.Random.Range(_lifeTime.MinLifeTimeSec, _lifeTime.MaxLifeTimeSec);
        transform.position = spawnPoint;
        _currentTime = 0;
        _isActive = true;
    }

    public virtual void UpdateMoveDir(Vector2 target) 
    {
        _movementStats.MovementDir = target - (Vector2)transform.position;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        int mask = 1 << collision.gameObject.layer;
        if((mask & _playerLayer) != 0) 
        {
            OnPlayerTouched();
            OnBackToThePool?.Invoke(this);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        int mask = 1 << collision.gameObject.layer;

        if ((mask & _borderLayer.value) != 0)
        {
            Vector2 normal = collision.GetContact(0).normal;
            _movementStats.MovementDir = Vector2.Reflect(_movementStats.MovementDir, normal).normalized;
        }
        else if ((mask & _playerLayer) != 0)
        {
            OnPlayerTouched();
            OnBackToThePool?.Invoke(this);
        }
    }

    protected virtual void OnPlayerTouched() 
    {
        
    }
}

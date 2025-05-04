using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public static event Action<CustomerController> OnCustomerFinished;

    [SerializeField]
    private TextMeshProUGUI _foodText;

    [SerializeField] 
    private float moveSpeed = 2f;

    [SerializeField]
    private Rigidbody2D _rb;

    private List<Transform> _path;
    private int _pathIndex = 0;
    private Quaternion _originalRotation;
    private Vector3 _originalScale;
    private FoodType _currentFoodType;

    public void SetUp(List<Transform> path) 
    {
        _path = path;
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
    }

    public void UpdateMoveDir() 
    {
        Vector2 targetPos = _path[_pathIndex].position;
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        _rb.linearVelocity = direction * moveSpeed;
    }

    public void ResetCustomer(Vector2 startingPoint)
    {
        transform.position = startingPoint;
        transform.rotation = _originalRotation;
        transform.localScale = _originalScale;

        _pathIndex = 0;
        SelectFood();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_path != null && _pathIndex < _path.Count && 
            other.transform == _path[_pathIndex])
        {
            _pathIndex++;
            if (_pathIndex >= _path.Count)
            {
                _rb.linearVelocity = Vector2.zero;
                OnCustomerFinished?.Invoke(this);
            }
            else 
                UpdateMoveDir();
        }
    }

    private void SelectFood()
    {
        if (!_foodText)
            return;

        FoodType[] values = (FoodType[])Enum.GetValues(typeof(FoodType));
        _currentFoodType = values[UnityEngine.Random.Range(0, values.Length)];

        _foodText.text = _currentFoodType.ToString();
    }
}

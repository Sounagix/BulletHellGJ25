using System;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private bool _active = false;

    [SerializeField]
    private ServedFood _foodPrefab;

    [SerializeField]
    private float _projectileSpeed;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerMouseLeftClickedDown += OnMouseLeftClickedDown;
        PlayerController.OnPlayerMouseLeftClickedUp += OnMouseLeftClickedUp;
        InventoryManager.EventShootFood += OnShootFood;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerMouseLeftClickedDown -= OnMouseLeftClickedDown;
        PlayerController.OnPlayerMouseLeftClickedUp -= OnMouseLeftClickedUp;
        InventoryManager.EventShootFood -= OnShootFood;
    }


    private void Update()
    {
        if (_active)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, mousePos);
        }
    }

    private void OnMouseLeftClickedUp()
    {
        _active = false;
        _lineRenderer.enabled = false;
    }

    private void OnMouseLeftClickedDown()
    {
        _active = true;
        _lineRenderer.enabled = true;
    }

    private void OnShootFood(ThroweableFood food)
    {
        GameObject currentFood = Instantiate(_foodPrefab.gameObject, transform.position, Quaternion.identity);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - transform.position).normalized;
        currentFood.GetComponent<ServedFood>().SetUp(food, dir, _projectileSpeed);
    }
}

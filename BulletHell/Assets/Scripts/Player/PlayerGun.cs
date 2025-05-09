using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private float _projectileForce;

    [SerializeField]
    private InteractablePool _foodPool;

    private bool _active = false;

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

    private void OnShootFood(ThroweableFood throweableFood)
    {
        MasterAudioManager.Instance.PlayOneShot(THROWEABLE_SOUND.THROW, transform);
        FoodController food = (FoodController)_foodPool.GetFromPool();
        food.ResetObject(transform.position, isPlayerOwner: true, throweableFood);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        food.UpdateTargetPosition(mousePos);
        food.UpdateProjectileForce(_projectileForce);
        TutorialManager.OnTutorialUpdate?.Invoke(TUTORIAL.ATTACK);
    }
}

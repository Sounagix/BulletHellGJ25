using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomerGraphics : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField]
    private SpriteRenderer _foodSprite;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private SpriteRenderer _highlightedRenderer;

    [SerializeField]
    private SpriteRenderer _unstableRenderer;

    [SerializeField]
    private Slider _patienceSlider;

    [Header("Increase Size Animation")]
    [SerializeField]
    private float _sizeIncrease;

    [SerializeField]
    private float _increaseInThisSeconds;

    //private CustomerRenderer _currentCustomerRenderer;
    private Vector3 _originalScale;
    private Color _originalColor;
    private CustomerController _customerController;
    

    public void SetUp(CustomerController controller)
    {
        _originalColor = _renderer.color;
        _customerController = controller;
    }

    public void ResetCustomer(/*CustomerRenderer customerRenderer,*/ Sprite food, float maxPatience) 
    {
        _highlightedRenderer.gameObject.SetActive(false);
        _unstableRenderer.gameObject.SetActive(false);
        _foodSprite.sprite = food;
        _patienceSlider.maxValue = maxPatience;
        _patienceSlider.value = maxPatience;
    }

    private void Update()
    {
        if(_customerController)
            ControlHighLight();
    }

    private void ControlHighLight()
    {
        _highlightedRenderer.sprite = _renderer.sprite;
        _unstableRenderer.sprite = _renderer.sprite;
    }

    public void TurnUnstable()
    {
        _unstableRenderer.gameObject.SetActive(true);
        _patienceSlider.gameObject.SetActive(false);
        StartCoroutine(IncreaseSize(_customerController.transform.localScale * _sizeIncrease, _increaseInThisSeconds));
    }

    private IEnumerator IncreaseSize(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = _customerController.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _customerController.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _customerController.transform.localScale = targetScale; // Asegura que llegue exactamente al tamaño final
    }

    public void UpdatePatienceBar(float _currentPatienceTime)
    {
        _patienceSlider.value -= _currentPatienceTime;
    }

    public void FadeOut(Action onComplete)
    {
        OnHighlightCustomer(false);
        StartCoroutine(FadeOutCoroutine(onComplete));
    }

    private IEnumerator FadeOutCoroutine(Action onComplete)
    {
        Color color = _renderer.color;
        float duration = 1f;
        float elapsed = 0f;

        while (_renderer.color.a > 0f)
        {
            float t = elapsed / duration;
            color.a = Mathf.Lerp(1f, 0f, t);
            _renderer.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = 0f;
        _renderer.color = color;
        onComplete?.Invoke();
    }

    public void OnHighlightCustomer(bool isHighlighted)
    {
        _highlightedRenderer.gameObject.SetActive(isHighlighted);
    }
}

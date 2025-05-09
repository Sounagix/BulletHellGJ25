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
    private Slider _patienceSlider;

    private CustomerRenderer _currentCustomerRenderer;

    public void SetUp(CustomerRenderer customerRenderer, Sprite food, float maxPatience)
    {
        // Customer Renderer
        _currentCustomerRenderer = customerRenderer;
        _highlightedRenderer.sprite = _renderer.sprite = _currentCustomerRenderer.NormalState;
        _highlightedRenderer.gameObject.SetActive(false);
        // Food & Patience
        _foodSprite.sprite = food;
        _patienceSlider.maxValue = maxPatience;
        _patienceSlider.value = maxPatience;
    }

    public void TurnUnstable()
    {
        _highlightedRenderer.sprite = _renderer.sprite = _currentCustomerRenderer.UnstableState;
        _patienceSlider.gameObject.SetActive(false);
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

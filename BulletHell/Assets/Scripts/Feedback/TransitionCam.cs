using System.Collections;
using UnityEngine;

public class TransitionCam : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image _fadeImage;

    [SerializeField]
    private float _fadeDuration;

    void Start()
    {
        StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        _fadeImage.gameObject.SetActive(true);
        float t = 0f;
        Color color = _fadeImage.color;

        while (t < _fadeDuration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / _fadeDuration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);
            _fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        _fadeImage.color = color;
        _fadeImage.gameObject.SetActive(false);
    }
}

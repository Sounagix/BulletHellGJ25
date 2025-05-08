using System;
using System.Collections;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public static Action ShakeCam;

    [SerializeField]
    private float _shakeDuration;

    [SerializeField]
    private float _shakeMagnitude;

    private Coroutine _shakeCoroutine;

    private void OnEnable()
    {
        ShakeCam += StartShake;
    }

    private void OnDisable()
    {
        ShakeCam -= StartShake;
    }

    private void StartShake()
    {
        if (_shakeCoroutine == null)
        {
            _shakeCoroutine = StartCoroutine(ShakeCamera(_shakeDuration, _shakeMagnitude));
        }
    }

    private IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            Camera.main.transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
        _shakeCoroutine = null;
    }
}

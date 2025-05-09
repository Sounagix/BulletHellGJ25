using System;
using System.Collections;
using UnityEngine;

public class GlowOnPick : MonoBehaviour
{
    public static Action OnGlowActive;

    [SerializeField]
    private Material _normalMat;

    [SerializeField]
    private Material _glowMat;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _glowDuration = 1.5f;

    private Coroutine _glowCoroutine;

    private void OnEnable()
    {
        OnGlowActive += ActiveGlow;
    }

    private void OnDisable()
    {
        OnGlowActive -= ActiveGlow;
    }

    public void ActiveGlow()
    {
        if (_glowCoroutine == null)
        {
            _glowCoroutine = StartCoroutine(Glow());
        }
    }

    private IEnumerator Glow()
    {
        _spriteRenderer.material = _glowMat;
        yield return new WaitForSeconds(_glowDuration);
        _spriteRenderer.material = _normalMat;
        _glowCoroutine = null;
    }
}

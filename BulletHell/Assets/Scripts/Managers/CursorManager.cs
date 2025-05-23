using System.Collections;
using TMPro;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D UITexture;

    [SerializeField]
    private Texture2D gameTexture;

    private static CursorManager Instance;

    public CursorManager GetInstance()
    {
        if (Instance == null)
            Instance = this;
        return Instance;
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static IEnumerator ChangeCursorTexture(GameState gameState)
    {
        yield return new WaitUntil(() => Instance);

        Texture2D newCursor = null;
        switch (gameState)
        {
            case GameState.Opening:
            case GameState.Pause:
            case GameState.GameOver:
                newCursor = Instance.UITexture;
                break;
            case GameState.Playing:
                newCursor = Instance.gameTexture;
                break;
        }

        if (newCursor != null)
        {
            Vector2 hotspot = new Vector2(newCursor.width / 2f, newCursor.height / 2f);

#if UNITY_WEBGL
            Cursor.SetCursor(newCursor, hotspot, CursorMode.Auto);
#else
            Cursor.SetCursor(newCursor, hotspot, CursorMode.Auto);
#endif
        }
        else
            Debug.LogWarning("CursorManager - Cursor texture is null! Keeping the current cursor.");
    }
}

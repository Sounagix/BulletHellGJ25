using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    #region Editor Variables

    [SerializeField]
    private Slider _loadingBar;

    [SerializeField]
    private GameObject _loadingCanvas;

    #endregion

    // Insert your Public Variables in this region
    #region Public Variables

    #endregion

    // Insert your Private Variables in this region
    #region Private Variables

    private GameManager _gameManager;

    private List<AsyncOperation> _scenesLoading = new List<AsyncOperation>();

    private float _totalProgress = 0f;

    #endregion

    #region Manager Implementation

    public void Initialize()
    {
        _gameManager = GameManager.Instance;
        ToggleBetweenLoadingCanvasOnOff(false);
        if (!_loadingBar)
        {
            CustomLog.LogError("Loading Bar is not assigned in the inspector");
            return;
        }

        _loadingBar.maxValue = 100f;
    }

    public void Shutdown()
    {

    }

    #endregion

    private void Start()
    {
        Initialize();
    }

    public void LoadGameScene()
    {
        ToggleBetweenLoadingCanvasOnOff(true);

        UnloadSceneAsync((int)SceneID.Menu);
        LoadSceneAsync((int)SceneID.Game);

        StartCoroutine(UpdateProgress());
    }

    public void ReturnToMenuScene()
    {
        ToggleBetweenLoadingCanvasOnOff(true);

        UnloadSceneAsync((int)SceneID.Game);
        LoadSceneAsync((int)SceneID.Menu);

        StartCoroutine(UpdateProgress());
    }

    public IEnumerator UpdateProgress()
    {
        SetLoadingBarValue(0);
        bool isLoading = true;

        while (isLoading)
        {
            _totalProgress = 0f;
            isLoading = false;

            foreach (AsyncOperation scene in _scenesLoading)
            {
                _totalProgress += scene.progress;
                if (!scene.isDone)
                    isLoading = true;
            }

            if (_scenesLoading.Count > 0)
            {
                _totalProgress = (_totalProgress / _scenesLoading.Count) * 100f;
                SetLoadingBarValue(Mathf.Round(_totalProgress));
            }

            yield return null;  // Wait for the next frame before checking again
        }

        ToggleBetweenLoadingCanvasOnOff(false);
        _scenesLoading.Clear();
    }

    private void ToggleBetweenLoadingCanvasOnOff(bool isOn)
    {
        if (_loadingCanvas == null)
        {
            CustomLog.Log("Loading Canvas is not assigned in the inspector", CustomLogColor.Red);
            return;
        }

        _loadingCanvas.SetActive(isOn);
    }

    private void SetLoadingBarValue(float value)
    {
        if (_loadingBar == null)
        {
            CustomLog.Log("Loading Bar is not assigned in the inspector", CustomLogColor.Red);
            return;
        }
        _loadingBar.value = value;
    }

    private void UnloadSceneAsync(int sceneIndex)
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(sceneIndex);

        if (!scene.isLoaded)
        {
            CustomLog.LogError($"Scene with index {sceneIndex} is not loaded and cannot be unloaded");
            return;
        }

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneIndex);
        if (unloadOperation != null)
            _scenesLoading.Add(unloadOperation);
        else
            CustomLog.LogError($"Failed to unload scene with index {sceneIndex}");
    }

    private void LoadSceneAsync(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            CustomLog.LogError($"Scene index {sceneIndex} is out of bounds. Check your build settings");
            return;
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        if (loadOperation != null)
            _scenesLoading.Add(loadOperation);
        else
            CustomLog.LogError($"Failed to load scene with index {sceneIndex}");
    }
}

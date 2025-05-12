using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Editor Variables

    [SerializeField]
    private SceneLoader _sceneLoader;

    [SerializeField]
    private MenuSceneManager _mainMenuSceneManager;

    [SerializeField]
    private GameSceneManager _gameSceneManager;

    [SerializeField]
    private bool _enableCustomLogs;

    #endregion

    #region Public Variables
    #endregion

    #region Private Variables

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();
                if (_instance == null)
                {
                    CustomLog.LogError("GameManager instance is null");
                    throw new InvalidOperationException("GameManager instance is not initialized");
                }
            }

            return _instance;
        }
    }

    private SceneID _initialScene = SceneID.LoadingScene;

    #endregion

    public void Awake()
    {
        if (_instance)
        {
            MoveGameManagerDataBeforeDestroy();
        }
        else
        {
            _instance = this;
            CustomLog.EnableDebugLog = _enableCustomLogs;

#if UNITY_EDITOR
            OnEditorDetermineInitialScene();
#endif

            InitializeGame();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void MoveGameManagerDataBeforeDestroy()
    {
        if (_mainMenuSceneManager)
            _instance._mainMenuSceneManager = _mainMenuSceneManager;
        else if (_gameSceneManager)
            _instance._gameSceneManager = _gameSceneManager;

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnEditorDetermineInitialScene()
    {
        CustomLog.Log($"Foundation Architecture loaded - wm");

        if (_sceneLoader)
            _initialScene = SceneID.LoadingScene;
        else if (_mainMenuSceneManager)
            _initialScene = SceneID.Menu;
        else if (_gameSceneManager)
            _initialScene = SceneID.Game;
    }
#endif

    /// <summary>
    /// Initialize the game by loading the menu scene using the scene loader.
    /// <para></para>
    /// In the case the game is initalized in the menu or in the game scene,
    /// this functionality won't be used
    /// </summary>
    private void InitializeGame()
    {
        if (_initialScene != SceneID.LoadingScene)
            return;

        int mainMenuScene = (int)SceneID.Menu;
        if (string.IsNullOrEmpty(SceneUtility.GetScenePathByBuildIndex(mainMenuScene)))
        {
            CustomLog.LogError($"Scene index {mainMenuScene} is invalid or not added to build settings");
            return;
        }

        SceneManager.LoadSceneAsync(mainMenuScene, LoadSceneMode.Additive);
    }

    public void ChangeSceneToGameScene()
    {
        if (_sceneLoader)
            _sceneLoader.LoadGameScene();
        else
            ChangeScene((int)SceneID.Game);
    }

    public void ChangeSceneToMenuScene()
    {
        if (_sceneLoader)
            _sceneLoader.ReturnToMenuScene();
        else
            ChangeScene((int)SceneID.Menu);
    }

    public void ChangeScene(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            CustomLog.LogError($"Scene index {sceneIndex} is invalid or not added to build settings");
            return;
        }

        SceneManager.LoadScene(sceneIndex);
    }

    public void ChangeCursorTexture(GameState state)
    {
        StartCoroutine(CursorManager.ChangeCursorTexture(state));
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }
}

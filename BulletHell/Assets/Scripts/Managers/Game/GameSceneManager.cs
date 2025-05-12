using FMODUnity;
using System;
using System.Collections;
using UnityEngine;

public class GameSceneManager : Manager
{
    public static event Action OnGamePaused;

    #region Editor Variables
    [SerializeField]
    private PlayerManager _playerManager;

    [SerializeField]
    private InteractableSpawnManager _interactableManager;

    [SerializeField]
    private PauseManager _pauseManager;

    [SerializeField]
    private CustomersManager _customersManager;

    [SerializeField]
    private HUDManager _hudManager;

    #endregion

    #region Public Variables
    #endregion

    #region Private Variables

    private GameState _gameState = GameState.Opening;
    public GameState GameState { get { return _gameState; } }

    private Coroutine _musicFadeInCoroutine = null;
    private Coroutine _musicFadeOutCoroutine = null;

    #endregion

    #region Manager Implementation

    public override void Initialize()
    {
        if (_isInitialized)
            return;

        _playerManager.GameSceneManager = this;
        _playerManager.Initialize();

        _interactableManager.Player = _playerManager;
        _interactableManager.Initialize();

        _customersManager.GameSceneManager = this;
        _customersManager.PlayerTr = _playerManager.transform;
        _customersManager.FoodData = _interactableManager.FoodData;
        _customersManager.Initialize();

        _hudManager.Player = _playerManager;
        _hudManager.Initialize();

        _pauseManager.SetUp(this);

        _gameState = GameState.Playing;
        GameManager.Instance.ChangeCursorTexture(_gameState);

        _isInitialized = true;
    }

    public override void Shutdown()
    {
    }

    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        LevelSceneManager.RequestLoadScene += OnRequestLoadScene;
    }

    private void OnDisable()
    {
        LevelSceneManager.RequestLoadScene -= OnRequestLoadScene;
    }

    void Start()
    {
        Initialize();
    }

    #endregion

    private void OnRequestLoadScene()
    {
        StartCoroutine(ResetGame());
    }

    public IEnumerator ResetGame()
    {
        GameManager.Instance.ChangeScene((int)SceneID.Game);
        yield return null;
    }

    public IEnumerator GameOver()
    {
        _gameState = GameState.GameOver;
        GameManager.Instance.ChangeCursorTexture(_gameState);
        GameManager.Instance.ChangeScene((int)SceneID.GameOver);
        yield return null;
    }

    public void PauseGame()
    {
        _gameState = GameState.Pause;
        GameManager.Instance.ChangeCursorTexture(_gameState);

        //HandleMusicFadeInFadeOut(pauseMusicEmitter, false, 2f, gamePlayMusicEmitter, true, 1f);

        Time.timeScale = 0;
        OnGamePaused?.Invoke();
    }

    public void ContinueGame()
    {
        _gameState = GameState.Playing;
        GameManager.Instance.ChangeCursorTexture(_gameState);

        //HandleMusicFadeInFadeOut(gamePlayMusicEmitter, true, 1f, pauseMusicEmitter, false, 1f);

        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        if (_musicFadeInCoroutine != null)
            StopCoroutine(_musicFadeInCoroutine);
        if (_musicFadeOutCoroutine != null)
            StopCoroutine(_musicFadeOutCoroutine);

        //_gamePlayMusicEmitter.Stop();
        //_pauseMusicEmitter.Stop();
        //_gameOverMusicEmitter.Stop();

        GameManager.Instance.ChangeSceneToMenuScene();
        Time.timeScale = 1;
    }

    private void HandleMusicFadeInFadeOut(StudioEventEmitter musicIn, bool isContinued, float fadeInSeconds,
                                        StudioEventEmitter musicOut, bool isPaused, float fadeOutSeconds)
    {
        if (_musicFadeInCoroutine != null)
            StopCoroutine(_musicFadeInCoroutine);
        _musicFadeInCoroutine = StartCoroutine(AudioManager.MusicFadeIn(musicIn, isContinued, fadeInSeconds));

        if (_musicFadeOutCoroutine != null)
            StopCoroutine(_musicFadeOutCoroutine);
        _musicFadeOutCoroutine = StartCoroutine(AudioManager.MusicFadeOut(musicOut, isPaused, fadeOutSeconds));
    }
}

using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Legacy;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSceneManager : Manager
{
    #region Editor Variables

    [SerializeField]
    private SettingsManager _settingsManager;

    [SerializeField]
    private GameObject _mainMenuCanvas;

    [SerializeField]
    private Button startGameButton;

    [SerializeField]
    private Button settingsButton;

    [SerializeField]
    private Button quitGameButton;

    [SerializeField]
    private Button _tutorialButton;


    [SerializeField]
    private TextMeshProUGUI _tutorialText;

    [SerializeField]
    private Color[] _tutorialTextColors;

    #endregion

    #region Public Variables
    #endregion

    #region Private Variables

    GameManager _gameManager;

    #endregion

    #region Manager Implementation

    public override void Initialize()
    {
        if (_isInitialized)
            return;

        _gameManager = GameManager.Instance;

        UIHelper.AddListenerToButton(startGameButton, StartGameButtonAction);

        UIHelper.AddListenerToButton(settingsButton, SettingsButtonAction);

        StartTutorialButton();

#if UNITY_WEBGL
        if (quitGameButton)
            quitGameButton.gameObject.SetActive(false);
#else
        UIHelper.AddListenerToButton(quitGameButton, QuitGameButtonAction);
#endif
        InitializeManagers();

        base._isInitialized = true;

        MasterAudioManager.Instance.PlayOneShot(OST_SOUND.MAIN_MENU, transform);

    }

    public override void Shutdown()
    {
        UIHelper.RemoveListenerFromButton(startGameButton, StartGameButtonAction);
        UIHelper.RemoveListenerFromButton(settingsButton, SettingsButtonAction);

#if !UNITY_WEBGL
        UIHelper.RemoveListenerFromButton(quitGameButton, QuitGameButtonAction);
#endif
    }

    #endregion

    #region Unity Callbacks

    private void OnEnable()
    {
        SettingsManager.OnSettingsOnOff += HandleSettingsToggle;
    }

    private void OnDisable()
    {
        SettingsManager.OnSettingsOnOff -= HandleSettingsToggle;
    }

    private void Start()
    {
        Initialize();
    }

    #endregion

    private void HandleSettingsToggle(bool isSettingsOpen)
    {
        if (!_mainMenuCanvas)
        {
            CustomLog.LogError("Main Menu Canvas is not assigned in the inspector");
            return;
        }

        _mainMenuCanvas.SetActive(!isSettingsOpen);
    }

    private void InitializeManagers()
    {
        if (!_settingsManager)
            CustomLog.LogError("Settings Manager is not assigned in the inspector");
        else
            _settingsManager.Initialize();
    }


    private void StartGameButtonAction()
    {
        _gameManager.ChangeSceneToGameScene();
    }

    private void SettingsButtonAction()
    {
        if (!_settingsManager)
        {
            CustomLog.LogError("Settings Manager is not assigned in the inspector");
            return;
        }

        _settingsManager.ToggleSettings(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void QuitGameButtonAction()
    {
        _gameManager.QuitGame();
    }

    private void StartTutorialButton()
    {
        UIHelper.AddListenerToButton(_tutorialButton, ToggleBetweenTutorialOnOff);

        bool isTutorialActive = GameManager.IsTutorialActive;

        _tutorialText.text = isTutorialActive ? "Tutorial is enabled" : "Tutorial is disabled";
        _tutorialText.color = isTutorialActive ? _tutorialTextColors[0] : _tutorialTextColors[1];
    }

    public void ToggleBetweenTutorialOnOff()
    {
        bool isTutorialActive = !GameManager.IsTutorialActive;

        _tutorialText.text = isTutorialActive ? "Tutorial is enabled" : "Tutorial is disabled";
        _tutorialText.color = isTutorialActive ? _tutorialTextColors[0] : _tutorialTextColors[1];

        GameManager.IsTutorialActive = isTutorialActive;
        EventSystem.current.SetSelectedGameObject(null);
    }

}

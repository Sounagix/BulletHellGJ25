using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseCanvas;

    [SerializeField]
    private Button continueGameButton;

    [SerializeField]
    private Button mainMenuButton;

    private GameSceneManager _gameSceneManager;

    public void SetUp(GameSceneManager manager)
    {
        _gameSceneManager = manager;
        pauseCanvas.SetActive(false);
        continueGameButton.onClick.AddListener(delegate { ContinueGame(); });
        mainMenuButton.onClick.AddListener(delegate { ReturnToMainMenu(); });
    }

    private void OnEnable()
    {
        GameSceneManager.OnGamePaused += OnGamePaused;
    }

    private void OnDisable()
    {
        GameSceneManager.OnGamePaused -= OnGamePaused;
    }

    private void ContinueGame()
    {
        pauseCanvas.SetActive(false);
        _gameSceneManager.ContinueGame();
    }

    private void OnGamePaused()
    {
        pauseCanvas.SetActive(true);
    }

    private void ReturnToMainMenu()
    {
        _gameSceneManager.ReturnToMainMenu();
    }
}

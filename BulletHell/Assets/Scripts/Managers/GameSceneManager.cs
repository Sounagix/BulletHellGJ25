using System.Collections;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    private PlayerManager _playerManager;

    [SerializeField]
    private InteractableSpawnManager _interactableManager;

    [SerializeField]
    private CustomersManager _customersManager;

    [SerializeField]
    private HUDManager _hudManager;

    #endregion

    #region Public Variables
    #endregion

    #region Private Variables
    #endregion

    #region Manager Implementation

    public void Initialize()
    {
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
    }

    public void Shutdown()
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
        GameManager.Instance.ChangeScene((int)SceneID.GameOver);
        yield return null;
    }
}

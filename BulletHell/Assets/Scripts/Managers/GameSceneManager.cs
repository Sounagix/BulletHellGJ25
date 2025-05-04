using System.Collections;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    private PlayerManager _playerManager;

    [SerializeField]
    private InteractableManager _interactableManager;

    [SerializeField]
    private CustomersManager _customersManager;

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
        _interactableManager.GameSceneManager = this;
        _interactableManager.Initialize();

        _customersManager.GameSceneManager = this;
        _customersManager.Initialize();
    }

    public void Shutdown()
    {
    }

    #endregion

    #region Unity Callbacks

    void Start()
    {
        Initialize();
    }

    #endregion

    public IEnumerator GameOver() 
    {
        yield return null;
    }
}

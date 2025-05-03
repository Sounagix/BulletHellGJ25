using System.Collections;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    private PlayerManager _playerManager;

    #endregion

    #region Public Variables
    #endregion

    #region Private Variables
    #endregion

    #region Manager Implementation

    public void Initialize()
    {
        _playerManager.SetUp(this);
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

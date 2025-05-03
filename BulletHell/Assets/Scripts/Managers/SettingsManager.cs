using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Manager
{
    #region Editor Variables

    [SerializeField]
    private GameObject _settingsCanvas;

    [SerializeField]
    private Button _closeSettingsButton;

    #endregion

    #region Public Variables

    public static event Action<bool> OnSettingsOnOff;

    #endregion

    #region Protected Variables

    #endregion

    #region Private Variables


    #endregion

    #region Manager Implementation

    public override void Initialize()
    {
        if (!_settingsCanvas)
        {
            CustomLog.LogError("Settings Canvas is not assigned in the inspector");
            return;
        }
        else
        {
            _settingsCanvas.SetActive(false);
        }

        UIHelper.AddListenerToButton(_closeSettingsButton, ToggleSettings, false);
    }

    public override void Shutdown()
    {
        if (!_settingsCanvas)
        {
            CustomLog.LogError("Settings Canvas is not assigned in the inspector");
            return;
        }

        UIHelper.RemoveListenerFromButton(_closeSettingsButton, ToggleSettings, false);
    }

    #endregion

    #region Unity Callbacks

    #endregion

    public void ToggleSettings(bool isSettingsOpen)
    {
        _settingsCanvas.SetActive(isSettingsOpen);
        OnSettingsOnOff?.Invoke(isSettingsOpen);
    }
}

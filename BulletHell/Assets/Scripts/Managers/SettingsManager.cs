using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Manager
{
    // Insert your Editor Variables in this region
    #region Editor Variables

    [SerializeField]
    private GameObject _settingsCanvas;

    [SerializeField]
    private Button _closeSettingsButton;

    #endregion

    // Insert your Public Variables in this region
    #region Public Variables

    public static event Action<bool> OnSettingsOnOff;

    #endregion

    // Insert your Protected Variables in this region
    #region Protected Variables

    #endregion

    // Insert your Private Variables in this region
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

    // Insert your Unity Methods in this region
    #region Unity Callbacks

    #endregion

    public void ToggleSettings(bool isSettingsOpen)
    {
        _settingsCanvas.SetActive(isSettingsOpen);
        OnSettingsOnOff?.Invoke(isSettingsOpen);
    }
}

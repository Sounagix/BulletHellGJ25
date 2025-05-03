using UnityEngine.UI;

public class UIHelper
{
    public static void AddListenerToButton(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null)
        {
            CustomLog.LogError($"Button {button.name} is not assigned.");
            return;
        }
        else if (action == null)
        {
            CustomLog.LogError($"Action is null for button {button.name}.");
            return;
        }

        button.onClick.AddListener(action);
    }

    public static void AddListenerToButton<T>(Button button, UnityEngine.Events.UnityAction<T> action, T value)
    {
        if (button == null)
        {
            CustomLog.LogError($"Button {button.name} is not assigned.");
            return;
        }
        else if (action == null)
        {
            CustomLog.LogError($"Action is null for button {button.name}.");
            return;
        }

        UnityEngine.Events.UnityAction<T> listenerAction = (v) => action(v);
        button.onClick.AddListener(() => listenerAction(value));
    }

    public static void RemoveListenerFromButton(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null)
        {
            CustomLog.LogError($"Button {button.name} is not assigned.");
            return;
        }
        else if (action == null)
        {
            CustomLog.LogError($"Action is null for button {button.name}.");
            return;
        }

        button.onClick.RemoveListener(action);
    }

    public static void RemoveListenerFromButton<T>(Button button, UnityEngine.Events.UnityAction<T> action, T value)
    {
        if (button == null)
        {
            CustomLog.LogError($"Button {button.name} is not assigned.");
            return;
        }
        else if (action == null)
        {
            CustomLog.LogError($"Action is null for button {button.name}.");
            return;
        }

        UnityEngine.Events.UnityAction<T> listenerAction = (v) => action(v);
        button.onClick.RemoveListener(() => listenerAction(value));
    }
}

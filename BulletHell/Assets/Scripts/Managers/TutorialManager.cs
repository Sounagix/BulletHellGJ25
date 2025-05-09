using System;
using UnityEngine;
using UnityEngine.UI;

public enum TUTORIAL
{
    ENTER,
    MOVEMENT,
    DASH,
    COLLECT_FOOD,
    ATTACK,
    FEED_CUSTOMER,
    END,
    NULL,
}

public class TutorialManager : MonoBehaviour
{
    public static Action<TUTORIAL> OnTutorialUpdate;

    [SerializeField]
    private TMPro.TextMeshProUGUI _tutorialText;

    [SerializeField]
    private Button _aceptButton;

    [SerializeField]
    private Button _cancelButton;

    [SerializeField]
    private string _tutorialEnterText;

    [SerializeField]
    private string _tutorialMovementText;

    [SerializeField]
    private string _tutorialDashText;

    [SerializeField]
    private string _tutorialCollectFood;

    [SerializeField]
    private string _tutorialAttackText;

    [SerializeField]
    private string _tutorialFeedCustomerText;

    [SerializeField]
    private string _tutorialEndText;

    [SerializeField]
    private float _endTime;

    private TUTORIAL tUTORIAL_PHASE = TUTORIAL.ENTER;


    private void OnEnable()
    {
        OnTutorialUpdate += UpdateTutorial;
    }

    private void OnDisable()
    {
        OnTutorialUpdate -= UpdateTutorial;
    }


    private void Start()
    {
        _tutorialText.text = _tutorialEnterText;
        SetUpButton();
    }

    private void SetUpButton()
    {
        _aceptButton.onClick.AddListener(
            delegate ()
            {
                tUTORIAL_PHASE = TUTORIAL.MOVEMENT;
                _tutorialText.text = _tutorialMovementText;
                _aceptButton.gameObject.SetActive(false);
                _cancelButton.gameObject.SetActive(false);
            });
        _cancelButton.onClick.AddListener(
            delegate ()
            {
                _tutorialText.transform.parent.gameObject.SetActive(false);
                // startGame
            });
    }

    private void UpdateTutorial(TUTORIAL tUTORIAL)
    {
        if (!tUTORIAL_PHASE.Equals(tUTORIAL))
            return;

        switch (tUTORIAL)
        {
            case TUTORIAL.ENTER:
                break;
            case TUTORIAL.MOVEMENT:
                _tutorialText.text = _tutorialDashText;
                break;
            case TUTORIAL.DASH:
                _tutorialText.text = _tutorialCollectFood;
                break;
            case TUTORIAL.COLLECT_FOOD:
                _tutorialText.text = _tutorialAttackText;
                break;
            case TUTORIAL.ATTACK:
                _tutorialText.text = _tutorialFeedCustomerText;
                break;
            case TUTORIAL.FEED_CUSTOMER:
                _tutorialText.text = _tutorialEndText;
                Invoke(nameof(EndOfTutorial), _endTime);
                break;
            case TUTORIAL.END:
                break;
            case TUTORIAL.NULL:
                break;
        }
        tUTORIAL_PHASE++;
    }

    private void EndOfTutorial()
    {
        _tutorialText.transform.parent.gameObject.SetActive(false);
    }
}

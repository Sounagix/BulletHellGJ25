using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TutorialPhase
{
    MOVEMENT,
    DASH,
    COLLECT_FOOD,
    ATTACK,
    FEED_CUSTOMER,
}

public class TutorialManager : MonoBehaviour
{
    public static Action<TutorialPhase> OnSkipTutorialPhase;

    [SerializeField]
    private GameObject _tutorialPanel;

    [Header("Text to display")]
    [SerializeField]
    private TextMeshProUGUI _tutorialText;

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


    [Header("Tutorial Effects")]
    [SerializeField]
    private float _timePerTutorial = 3f;

    [SerializeField]
    private float _tutorialFadeDuration = 1f;

    [SerializeField] 
    private float _delayBetweenTutorials = 1f;


    private Dictionary<TutorialPhase, bool> _tutorialPhases;
    private Color _originalTextColor;
    private TutorialPhase _currentTutorialPhase;
    private bool _skipCurrentTutorial = false;

    private void OnEnable()
    {
        OnSkipTutorialPhase += OnTutorialPhaseSkipped;
    }

    private void OnDisable()
    {
        OnSkipTutorialPhase -= OnTutorialPhaseSkipped;
    }


    private void Start()
    {
        _originalTextColor = _tutorialText.color;
        bool isTutorialActive = GameManager.IsTutorialActive;
        _tutorialPhases = new Dictionary<TutorialPhase, bool>();
        foreach (TutorialPhase phase in Enum.GetValues(typeof(TutorialPhase)))
            _tutorialPhases[phase] = isTutorialActive;
        _tutorialPanel.SetActive(isTutorialActive);

        if (isTutorialActive)
            StartCoroutine(DisplayTutorial());
    }

    private void Update()
    {
        if (!GameManager.IsTutorialActive)
            return;

        DisplayTutorial();
    }


    private IEnumerator DisplayTutorial()
    {
        Color transparentColor = _originalTextColor;
        transparentColor.a = 0f;

        foreach (TutorialPhase phase in Enum.GetValues(typeof(TutorialPhase)))
        {
            bool isPhaseActive = _tutorialPhases[phase];
            if (isPhaseActive)
            {
                _currentTutorialPhase = phase;
                _skipCurrentTutorial = false;

                UpdateTutorial(phase);
                _tutorialText.color = transparentColor;

                // Fade in
                float elapsed = 0f;
                while (elapsed < _tutorialFadeDuration)
                {
                    _tutorialText.color = Color.Lerp(transparentColor, _originalTextColor, elapsed / _tutorialFadeDuration);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                _tutorialText.color = _originalTextColor;

                float showElapsed = 0f;
                while (showElapsed < _timePerTutorial && !_skipCurrentTutorial)
                {
                    showElapsed += Time.deltaTime;
                    yield return null;
                }

                // Fade out
                elapsed = 0f;
                while (elapsed < _tutorialFadeDuration)
                {
                    _tutorialText.color = Color.Lerp(_originalTextColor, transparentColor, elapsed / _tutorialFadeDuration);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                _tutorialText.color = transparentColor;

                yield return new WaitForSeconds(_delayBetweenTutorials);
            }
        }

        EndOfTutorial();
    }


    private void OnTutorialPhaseSkipped(TutorialPhase tutorialPhaseTriggered)
    {
        if (_currentTutorialPhase == tutorialPhaseTriggered)
            _skipCurrentTutorial = true;

        _tutorialPhases[tutorialPhaseTriggered] = false;
    }

    private void UpdateTutorial(TutorialPhase phase)
    {
        switch (phase)
        {
            case TutorialPhase.MOVEMENT:
                _tutorialText.text = _tutorialMovementText;
                break;
            case TutorialPhase.DASH:
                _tutorialText.text = _tutorialDashText;
                break;
            case TutorialPhase.COLLECT_FOOD:
                _tutorialText.text = _tutorialCollectFood;
                break;
            case TutorialPhase.ATTACK:
                _tutorialText.text = _tutorialAttackText;
                break;
            case TutorialPhase.FEED_CUSTOMER:
                _tutorialText.text = _tutorialFeedCustomerText;
                break;
        }

        _tutorialPhases[phase] = false;
    }

    private void EndOfTutorial()
    {
        _tutorialPanel.SetActive(false);
        GameManager.IsTutorialActive = false;
    }
}

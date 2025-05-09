using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _presicionText;
    
    [SerializeField]
    private TextMeshProUGUI _totalDishesDeliveredCorrectlyText;

    [SerializeField]
    private TextMeshProUGUI _totalIngredientCollectedText;

    [SerializeField]
    private TextMeshProUGUI _totalTimesPlayerDamagedText;

    [SerializeField]
    private Button _playerAgaiButton;

    private void Start()
    {
        _playerAgaiButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ChangeScene((int)SceneID.Game);
        });

        var statistics = StatisticsManager.Instance;
        _presicionText.text = $"{statistics.GetPresicion()}%";
        _totalDishesDeliveredCorrectlyText.text = statistics.GetTotalDishesDeliveredCorrectly().ToString();
        _totalIngredientCollectedText.text = statistics.GetTotalIngredientCollected().ToString();
        _totalTimesPlayerDamagedText.text = statistics.GetTotalTimesPlayerDamaged().ToString();
        statistics.ResetInfo();
    }
}

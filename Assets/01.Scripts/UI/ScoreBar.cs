    using TMPro;
    using UnityEngine;

    public class ScoreBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _blueScoreText, _redScoreText;

        void Start()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.blueScore.OnValueChanged += HandleBlueScoreChange;
            HandleBlueScoreChange(0, GameManager.Instance.blueScore.Value);
            GameManager.Instance.redScore.OnValueChanged += HandleRedScoreChange;
            HandleRedScoreChange(0, GameManager.Instance.redScore.Value);
        }

        private void HandleRedScoreChange(int previousValue, int newValue)
        {
            _redScoreText.text = newValue.ToString();
        }

        private void HandleBlueScoreChange(int previousValue, int newValue)
        {
            _blueScoreText.text = newValue.ToString();
        }

    }

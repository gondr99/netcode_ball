using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private Button _goToMenuBtn;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        SetActivePanel(false);
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnGameOverEvent += ShowWindow;
    }


    public void SetActivePanel(bool isOn)
    {
        _canvasGroup.interactable = isOn;
        _canvasGroup.blocksRaycasts = isOn;
        _canvasGroup.alpha = isOn ? 1f : 0;
    }

    public void ShowWindow(string titleMsg)
    {
        _titleText.text = titleMsg;
        SetActivePanel(true);
        _goToMenuBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.GoToMenuScene();
        });
    }
}

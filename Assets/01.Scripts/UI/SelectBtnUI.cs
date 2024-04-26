using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectBtnUI : MonoBehaviour
{
    [SerializeField] private CharDataSO _dataSO;
    [SerializeField] private Image _spriteImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    private Button _selectBtn;

    private void Awake()
    {
        _selectBtn = GetComponent<Button>();
        _selectBtn.onClick.AddListener(HandleCharacterSelect);
    }

    private void HandleCharacterSelect()
    {

    }

    private void OnValidate()
    {
        if (_dataSO == null) return;
        
        if(_spriteImage != null)
        {
            _spriteImage.sprite = _dataSO.idleSprite;
        }
        
        if(_nameText != null)
        {
            _nameText.text = _dataSO.charName;
        }
    }

}

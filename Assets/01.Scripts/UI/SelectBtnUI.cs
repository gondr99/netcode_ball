using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectBtnUI : MonoBehaviour
{
    [SerializeField] private CharDataSO _dataSO;
    [SerializeField] private Image _spriteImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    private Button _selectBtn;

    public event Action<SelectBtnUI> OnSelectedEvent;
    public Image selectImage;
    public CharDataSO DataSO => _dataSO;

    private void Awake()
    {
        _selectBtn = GetComponent<Button>();
        _selectBtn.onClick.AddListener(HandleCharacterSelect);
        selectImage = transform.Find("SelectFrame").GetComponent<Image>();
    }

    public void HandleCharacterSelect()
    {
        //Player status is ready! so fixed
        if (GameManager.Instance.ActivePlayer.isReady.Value) return;


        OnSelectedEvent?.Invoke(this);
        EventSystem.current.SetSelectedGameObject(null);
        GameManager.Instance.ActivePlayer.SelectCharacterServerRpc(_dataSO.characterIndex);
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

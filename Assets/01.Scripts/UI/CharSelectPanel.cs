using TMPro;
using UnityEngine;

public class CharSelectPanel : MonoBehaviour
{
    [SerializeField] private SelectBtnUI[] _btnList;
    [SerializeField] private TextMeshProUGUI _infoText;

    private void Start()
    {
        for (int i = 0; i < _btnList.Length; i++)
        {
            _btnList[i].OnSelectedEvent += HandleSelectEvent;
            _btnList[i].selectImage.enabled = false;
        }

        HandleSelectEvent(_btnList[0]);
    }

    private void HandleSelectEvent(SelectBtnUI ui)
    {
        for (int i = 0; i < _btnList.Length; i++)
        {
            _btnList[i].selectImage.enabled = false;
        }
        ui.selectImage.enabled = true;

        _infoText.text = ui.DataSO.GetInfoString();
    }

    #region only debug
    public void SelectPlayer(int index)
    {
        _btnList[index].HandleCharacterSelect();
    }
    #endregion
}

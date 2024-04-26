using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ReadyPanel : NetworkBehaviour
{
    [SerializeField] private TMP_InputField _tmpNameInput;
    [SerializeField] private Button _readyBtn, _startBtn;

    private bool _isReady = false;

    [SerializeField] private Sprite[] _readyBtnSprite;
    private TextMeshProUGUI _readyBtnText;

    private void Awake()
    {
        _readyBtnText = _readyBtn.GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void OnNetworkSpawn()
    {
        _startBtn.interactable = false;
        _readyBtn.interactable = false;
        GameManager.Instance.OnActivePlayerSetEvent += HandleSetActivePlayer;
        HandleSetActivePlayer(GameManager.Instance.ActivePlayer);

        _readyBtn.onClick.AddListener(HandleReadyBtnClick);
    }

    //after active player set, ready button will be enabled!
    private void HandleSetActivePlayer(Player player)
    {
        if(player != null)
        {
            _readyBtn.interactable = true;
        }
    }

    public override void OnNetworkDespawn()
    {
        _readyBtn.onClick.RemoveListener(HandleReadyBtnClick);
    }

    private void HandleReadyBtnClick()
    {
        if (_tmpNameInput.text.Length > 6 || _tmpNameInput.text.Length < 2)
        {
            Debug.Log("Invalid playerName");
            return;
        }

        FixedString64Bytes name = _tmpNameInput.text;
        _isReady = !_isReady;
        ulong clientID = NetworkManager.Singleton.LocalClientId;
        SetReadyStatusServerRpc(clientID, name, _isReady);

        _tmpNameInput.interactable = !_isReady;

        _readyBtnText.text = _isReady ? "On Ready!" : "Ready";
        _readyBtn.image.sprite = _isReady ? _readyBtnSprite[1] : _readyBtnSprite[0];
    }

    #region only server area
    [ServerRpc(RequireOwnership = false)]
    public void SetReadyStatusServerRpc(ulong clientID, FixedString64Bytes name, bool status)
    {
        if(GameManager.Instance.SetReady(name, status, clientID))
        {
            _startBtn.interactable = true; //호스트만 켜준다.
        }
        else
        {
            _startBtn.interactable = false;
        }
    }
    #endregion
}

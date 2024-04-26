using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Transform _selectSceneSpawnPosition;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private CharDataListSO _charDataList;

    public List<CharDataSO> CharList => _charDataList.list;

    public event Action<Player> OnActivePlayerSetEvent;

    private Player _activePlayer;//currently active player object
    public Player ActivePlayer {
        get => _activePlayer;
        set
        {
            if(_activePlayer != value)
            {
                _activePlayer = value;
                OnActivePlayerSetEvent?.Invoke(_activePlayer);
            }
        }
    }

    #region Only Server
    public Dictionary<ulong, Player> playerDictionary;

    private void Awake()
    {
        playerDictionary = new Dictionary<ulong, Player>();
    }

    public void SpawnPlayerInSelectScene(ulong clientID)
    {
        Player player = Instantiate(_playerPrefab, _selectSceneSpawnPosition.position, Quaternion.identity);
        player.NetworkObject.SpawnAsPlayerObject(clientID);

        playerDictionary.Add(clientID, player);
    }

    //SelectCharacter - call by rpc
    public void SelectCharacter(int index, ulong clientID)
    {
        //Set network variable by server
        playerDictionary[clientID].selectCharacterIndex.Value = index;
        UserData data =  AppHost.Instance.NetServer.GetUserData(clientID);
        data.characterIndex = index;
        AppHost.Instance.NetServer.SetUserData(clientID, data);
    }

    public bool SetReady(FixedString64Bytes userName, bool isReady, ulong clientID)
    {
        Player player = playerDictionary[clientID];
        player.userName.Value = userName;
        player.isReady.Value = isReady;

        UserData data = AppHost.Instance.NetServer.GetUserData(clientID);
        data.playerName = userName;
        AppHost.Instance.NetServer.SetUserData(clientID, data);

        return playerDictionary.Count == 2 
            && playerDictionary.Values.Any(p => p.isReady.Value == false) == false ;
    }

    #endregion

    public CharDataSO GetCharDataByIndex(int index)
    {
        return CharList[index];
    }

}

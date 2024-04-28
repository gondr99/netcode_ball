using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Transform _selectSceneSpawnPosition;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private CharDataListSO _charDataList;

    public List<CharDataSO> CharList => _charDataList.list;

    public event Action<Player> OnActivePlayerSetEvent;

    private Player _activePlayer;//currently active player object
    public Player ActivePlayer
    {
        get => _activePlayer;
        set
        {
            if (_activePlayer != value)
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
        NetworkManager.Singleton.SceneManager.OnLoadComplete += HandleSceneLoadComplete;
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= HandleSceneLoadComplete;
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
        UserData data = AppHost.Instance.NetServer.GetUserData(clientID);
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
            && playerDictionary.Values.Any(p => p.isReady.Value == false) == false;
    }


    public void StartGame()
    {
        DontDestroyOnLoad(gameObject);
        NetworkManager.Singleton.SceneManager.LoadScene(SceneNames.Game, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void HandleSceneLoadComplete(ulong clientId, string sceneName, LoadSceneMode mode)
    {
        ulong serverID = NetworkManager.Singleton.LocalClientId;
        if (sceneName == SceneNames.Game && clientId == serverID)
        {
            MovePlayerInGameScene();
        }
    }

    public void MovePlayerInGameScene()
    {
        GameSpawnPoints SpawnPoints = GameObject.FindObjectOfType<GameSpawnPoints>();
        Vector3[] positions = SpawnPoints.GetPositions();
        int index = 0;
        foreach(Player p in playerDictionary.Values)
        {
            p.HealthCompo.ResetHealth();
            p.transform.position = positions[index];
            index++;
        }

    }

    public void GoToSelectScene()
    {
        //if goto the other scene then destroy gameobject
        IsDestoryed = true;
        Destroy(gameObject);
    }

    #endregion

    public CharDataSO GetCharDataByIndex(int index)
    {
        return CharList[index];
    }

}

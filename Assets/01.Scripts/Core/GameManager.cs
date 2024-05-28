using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetMonoSingleton<GameManager>
{
    [SerializeField] private Transform _selectSceneSpawnPosition;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private CharDataListSO _charDataList;
    [SerializeField] private int _winScore = 5;

    public List<CharDataSO> CharList => _charDataList.list;

    public event Action<Player> OnActivePlayerSetEvent;
    
    public NetworkVariable<int> blueScore;
    public NetworkVariable<int> redScore;

    public event Action<string> OnGameOverEvent;
    public bool isGameEnd = false;

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

    public Dictionary<ulong, Player> playerDictionary;

    private void Awake()
    {
        
        playerDictionary = new Dictionary<ulong, Player>();
        if (NetworkManager.Singleton == null) return;
        blueScore = new NetworkVariable<int>(0);
        redScore = new NetworkVariable<int>(0);

    }

    public override void OnNetworkSpawn()
    {
        DontDestroyOnLoad(gameObject);
        if (!IsHost) return;
        NetworkManager.Singleton.SceneManager.OnLoadComplete += HandleSceneLoadComplete;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsHost) return;
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= HandleSceneLoadComplete;
    }

    #region Only Server

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

        return IsAllReady();
    }

    public bool IsAllReady()
    {
        return playerDictionary.Count == 2
            && playerDictionary.Values.Any(p => p.isReady.Value == false) == false;
    }


    public void StartGame()
    {
        
        NetworkManager.Singleton.SceneManager.LoadScene(SceneNames.Game, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void HandleSceneLoadComplete(ulong clientId, string sceneName, LoadSceneMode mode)
    {
        ulong serverID = NetworkManager.Singleton.LocalClientId;
        if (sceneName == SceneNames.Game && clientId == serverID)
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        blueScore.Value = 0;
        redScore.Value = 0;
        isGameEnd = false;
        MovePlayerInGameScene();
    }

    public void MovePlayerInGameScene()
    {
        //if gamescene load complete!
        GameSpawnPoints SpawnPoints = GameObject.FindObjectOfType<GameSpawnPoints>();
        PlayerTeamInfo[] infos = SpawnPoints.GetPositions();
        int index = 0;
        foreach(Player p in playerDictionary.Values)
        {
            p.HealthCompo.ResetHealth();
            p.teamInfo = infos[index];
            p.MoveToPosition(infos[index].originPos);
            index++;
        }
        BallManager.Instance.OnScoreEvent += HandleScoreEvent;
        //여기 온건 씬 로딩이 끝난 상태라 Awake까지 모두 종료된 상태. 따라서 싱글톤 사용가능

        
        BallManager.Instance.GenerateBallInCountDown(5);
    }

    private void HandleScoreEvent(Team team)
    {
        if (team == Team.Blue)
            blueScore.Value++;
        else if (team == Team.Red)
            redScore.Value++;

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if(blueScore.Value >= _winScore)
        {
            isGameEnd = true;
            SetGameEndClientRpc("Blue team win!");
        }
        else if(redScore.Value >= _winScore)
        {
            isGameEnd = true;
            SetGameEndClientRpc("Red team win!");
        }
    }

    [ClientRpc]
    public void SetGameEndClientRpc(string msg)
    {
        OnGameOverEvent?.Invoke(msg);
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

    public void GoToMenuScene()
    {
        if(IsHost)
        {
            AppHost.Instance.ShutDownHost();
            SceneManager.LoadScene(SceneNames.MenuScene);
        }
        else
        {
            AppClient.Instance.Disconnect();
        }
        IsDestoryed = true;
        Destroy(gameObject);
    }
}

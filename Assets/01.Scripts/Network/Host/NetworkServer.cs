using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;

public class NetworkServer : IDisposable
{
    public Action<ulong> OnClientLeft;

    private NetworkManager _manager;

    private Dictionary<ulong, UserData> _userDictionary;

    public NetworkServer(NetworkManager manager)
    {
        _manager = manager;
        _userDictionary = new Dictionary<ulong, UserData>();

        _manager.ConnectionApprovalCallback += HandleConnectionApproval;
        _manager.OnServerStarted += HandleServerStarted;
    }


    private void HandleConnectionApproval(NetworkManager.ConnectionApprovalRequest req, NetworkManager.ConnectionApprovalResponse res)
    {
        if (_userDictionary.Count >= 2)
        {
            res.Approved = false;
            res.Reason = "The room is full";
            return;
        }

        UserData data = new UserData
        {
            clientID = req.ClientNetworkId,
            characterIndex = 0,
            playerName = $"Player_{req.ClientNetworkId}"
        };

        _userDictionary.Add(data.clientID, data);

        res.CreatePlayerObject = false;
        res.Approved = true;

        DelayedSpawnPlayer(data.clientID);
    }

    public UserData GetUserData(ulong clientID)
    {
        return _userDictionary[clientID];
    }

    public void SetUserData(ulong clientID, UserData data)
    {
        _userDictionary[clientID] = data;
    }

    private async void DelayedSpawnPlayer(ulong clientID)
    {
        //init time, server load networkserver first
        while(true)
        {
            await Task.Delay(100);
            if (GameManager.Instance != null) break;
        }
        GameManager.Instance.SpawnPlayerInSelectScene(clientID);
    }

    private void HandleServerStarted()
    {
        _manager.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void HandleClientDisconnect(ulong clientID)
    {
        if (_userDictionary.ContainsKey(clientID))
        {
            _userDictionary.Remove(clientID);
            OnClientLeft?.Invoke(clientID); //for after using purpose
        }
    }

    public void Dispose()
    {
        if (_manager == null) return;

        _manager.ConnectionApprovalCallback -= HandleConnectionApproval;
        _manager.OnServerStarted -= HandleServerStarted;

        _manager.OnClientDisconnectCallback -= HandleClientDisconnect;

        if (_manager.IsListening)
        {
            _manager.Shutdown();
        }
    }

}

using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkClient : IDisposable
{
    private NetworkManager _manager;

    public NetworkClient(NetworkManager manager)
    {
        _manager = manager;

        _manager.OnClientDisconnectCallback += HandleClientDisconnectCallback;
    }

    private void HandleClientDisconnectCallback(ulong clientID)
    {

        if (!_manager.IsServer && _manager.DisconnectReason != string.Empty)
        {
            Debug.Log($"Approval Declined Reason: {_manager.DisconnectReason}");
        }

        if (clientID != 0 && clientID != _manager.LocalClientId) return;

        Disconnect();

    }

    public void Disconnect()
    {
        //씬을 이동시켜주고 현재 접속되어 있다면 연결을 끊어준다.
        if (SceneManager.GetActiveScene().name != SceneNames.MenuScene)
        {
            SceneManager.LoadScene(SceneNames.MenuScene);
        }

        if (_manager.IsConnectedClient)
        {
            _manager.Shutdown();
        }
    }

    public void Dispose()
    {
        if (_manager != null)
        {
            _manager.OnClientDisconnectCallback -= HandleClientDisconnectCallback;
        }
    }

}

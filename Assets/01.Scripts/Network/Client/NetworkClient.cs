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
        //���� �̵������ְ� ���� ���ӵǾ� �ִٸ� ������ �����ش�.
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

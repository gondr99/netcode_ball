using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetTestDebugger : NetworkBehaviour
{
    private string _currentSceneName;

    private ReadyPanel _readyPanel;

    private void Start()
    {
        _currentSceneName = SceneManager.GetActiveScene().name;    
        if(_currentSceneName == SceneNames.Select)
        {
            _readyPanel = FindAnyObjectByType<ReadyPanel>(); //시작시 패널 가져오고
        }
    }
    private void Update()
    {
        switch (_currentSceneName)
        {
            case SceneNames.MenuScene:
                HandleMenuSceneTest();
                break;

            case SceneNames.Select:
                HandleSelectSceneTest();
                break;
        }
        
    }

    private void HandleSelectSceneTest()
    {
        if (!IsHost) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetReadyClientRpc();
            WaitingForGo();
        }
    }

    private async void WaitingForGo()
    {
        while(true)
        {
            await Task.Delay(200);
            if( GameManager.Instance.IsAllReady())
            {
                break;
            }
        }
        _readyPanel.StartGameWithHost();
    }

    [ClientRpc]
    public void SetReadyClientRpc()
    {
        if(IsHost)
        {
            _readyPanel.SetReadyWithNameAndCharacter(0, "Host");
        }
        else
        {
            _readyPanel.SetReadyWithNameAndCharacter(1, "Client");
        }
    }

    private void HandleMenuSceneTest()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetUpDefaultPassport();
            AppHost.Instance.StartHost();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetUpDefaultPassport();
            AppClient.Instance.StartClient();
        }
    }


    private void SetUpDefaultPassport()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>()
            .SetConnectionData("127.0.0.1", 8989);
    }
}

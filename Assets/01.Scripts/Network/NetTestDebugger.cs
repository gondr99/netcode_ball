using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetTestDebugger : MonoBehaviour
{
    private void Update()
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

using Unity.Netcode;

public class AppClient : MonoSingleton<AppClient>
{
    public NetworkClient NetClient { get; private set; }


    public void MakeClient()
    {
        NetClient = new NetworkClient(NetworkManager.Singleton);
    }

    public void StartClient()
    {
        MakeClient();
        NetworkManager.Singleton.StartClient();
    }

    public void Disconnect()
    {
        NetClient.Disconnect();
    }

    private void OnDestroy()
    {
        NetClient?.Dispose();
    }
}

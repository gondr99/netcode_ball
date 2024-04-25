using Unity.Netcode;
using UnityEngine.SceneManagement;

public class AppHost : MonoSingleton<AppHost>
{
    public NetworkServer NetServer { get; private set; }

    public void MakeServer()
    {
        NetServer = new NetworkServer(NetworkManager.Singleton);
    }

    public void StartHost()
    {
        MakeServer();
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(
            SceneNames.Select, LoadSceneMode.Single);
    }

    //Shut down host server by host
    public void ShutDownHost()
    {
        NetServer?.Dispose();
    }

    private void OnDestroy()
    {
        NetServer?.Dispose();
    }
}

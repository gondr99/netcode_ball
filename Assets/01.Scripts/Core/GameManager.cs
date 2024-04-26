using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Transform _selectSceneSpawnPosition;
    [SerializeField] private Player _playerPrefab;

    public void SpawnPlayerInSelectScene(ulong clientID)
    {
        Player player = Instantiate(_playerPrefab, _selectSceneSpawnPosition.position, Quaternion.identity);
        player.NetworkObject.SpawnAsPlayerObject(clientID);
    }
}

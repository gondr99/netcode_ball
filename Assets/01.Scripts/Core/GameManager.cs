using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Transform _selectSceneSpawnPosition;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private CharDataListSO _charDataList;

    public List<CharDataSO> CharList => _charDataList.list;

    public Player activePlayer; //현재 조종중인 플레이어

    public void SpawnPlayerInSelectScene(ulong clientID)
    {
        Player player = Instantiate(_playerPrefab, _selectSceneSpawnPosition.position, Quaternion.identity);
        player.NetworkObject.SpawnAsPlayerObject(clientID);
    }

    public CharDataSO GetCharDataByIndex(int index)
    {
        return CharList[index];
    }
}

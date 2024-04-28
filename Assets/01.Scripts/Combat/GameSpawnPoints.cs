using UnityEngine;

public class GameSpawnPoints : MonoBehaviour
{
    [SerializeField] private Transform _playerATrm;
    [SerializeField] private Transform _playerBTrm;
    [SerializeField] private Transform _ballSpawnTrm;

    public Vector3[] GetPositions()
    {
        return new Vector3[] { _playerATrm.position, _playerBTrm.position };
    }
    
}

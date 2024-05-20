using UnityEngine;

public class GameSpawnPoints : MonoBehaviour
{
    [SerializeField] private Transform _playerATrm;
    [SerializeField] private Transform _playerBTrm;
    [SerializeField] private Transform _ballSpawnTrm;

    public PlayerTeamInfo[] GetPositions()
    {
        return new PlayerTeamInfo[] {
            new PlayerTeamInfo {team = Team.Red, originPos = _playerATrm.position }, 
            new PlayerTeamInfo {team = Team.Blue, originPos = _playerBTrm.position }
        };
    }

    public Vector3 BallSpawnPosition => _ballSpawnTrm.position;
    
}

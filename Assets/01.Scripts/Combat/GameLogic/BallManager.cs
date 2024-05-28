using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class BallManager : NetworkBehaviour
{
    public static BallManager Instance;

    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private GameSpawnPoints _spawnPoints;
    public event Action<Team> OnScoreEvent;

    private void Awake()
    {
        Instance = this;
    }

    [ClientRpc]
    public void ShowTextMessageClientRpc(string msg, float time = 1f)
    {
        MsgCanvas.Instance.ShowMsg(msg, time);
        //Debug.Log(msg);
    }

    #region server only
    public void GenerateBallInCountDown(int count)
    {
        StartCoroutine(GenerateBallCoroutine(count));
    }

    private IEnumerator GenerateBallCoroutine(int count)
    {
        var ws = new WaitForSeconds(1f);
        for (int i = count; i > 0; i--)
        {
            yield return ws;
            ShowTextMessageClientRpc(i.ToString());
        }

        GenerateBall();
    }

    public void GenerateBall()
    {
        Vector3 position = _spawnPoints.BallSpawnPosition;
        Ball ball = Instantiate(_ballPrefab, position, Quaternion.identity);
        ball.OnCollideWithGroundEvent += HandleBallHitGround;

        ball.NetworkObject.Spawn(true); //DestroyWithScene
    }

    private void HandleBallHitGround(Ball ball, Team hitGroundOwner)
    {
        if(hitGroundOwner == Team.Red)
        {
            ShowTextMessageClientRpc("Blue Team : 1 Point!", 2f);
            OnScoreEvent?.Invoke(Team.Blue);
        }
        else
        {
            ShowTextMessageClientRpc("Red Team : 1 Point!", 2f);
            OnScoreEvent?.Invoke(Team.Red);
        }

        Destroy(ball.gameObject);
        if(!GameManager.Instance.isGameEnd)
            GenerateBallInCountDown(5);
    }
    #endregion
}

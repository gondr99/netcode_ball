using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : NetworkBehaviour
{
    public LayerMask whatIsProjectile;
    public event Action<Ball, Team> OnCollideWithGroundEvent;

    private Rigidbody2D _rbCompo;
    private void Awake()
    {
        _rbCompo = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsServer) return;

        if (CheckMask(collision.gameObject, whatIsProjectile)) //투사체와 충돌시
        {
            _rbCompo.AddForce(new Vector2(0, 7f), ForceMode2D.Impulse);
            float torque = Random.Range(0, 50f);
            _rbCompo.AddTorque(torque);
        }
    }

    public void OnTriggeredOnGround(Team groundOwner)
    {
        if (!IsServer) return;

        OnCollideWithGroundEvent?.Invoke(this, groundOwner);
        
    }


    private bool CheckMask(GameObject obj, LayerMask layerMask)
    {
        int mask = 1 << obj.layer;
        return (mask & layerMask) > 0;
    }
}

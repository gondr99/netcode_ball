using Unity.Netcode;
using UnityEngine;

public class TeamZone : NetworkBehaviour
{
    [SerializeField] private Team _team;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsHost) return;

        if (collision.TryGetComponent(out Player player))
        {
            if (player.teamInfo.team != _team)
            {
                Vector2 direction = transform.position - player.transform.position;
                player.HealthCompo.TakeDamage(10, direction.normalized, 20f);
            }
        }
    }
}

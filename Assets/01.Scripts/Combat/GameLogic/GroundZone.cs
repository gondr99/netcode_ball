using UnityEngine;

public class GroundZone : MonoBehaviour
{
    public Team groundOwner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Ball ball))
        {
            ball.OnTriggeredOnGround(groundOwner);
        }
    }
}

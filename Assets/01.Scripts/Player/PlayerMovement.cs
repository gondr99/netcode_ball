using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D RbCompo { get; private set; }
    [SerializeField] private Transform _groundCheckerTrm;
    [SerializeField] private Vector2 _groundCheckerSize;
    [SerializeField] private LayerMask _whatIsGround;


    public bool IsGround { get; private set; }

    private void Awake()
    {
        RbCompo = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        IsGround = CheckGrounded();
    }

    public bool CheckGrounded()
    {
        Collider2D collider = Physics2D.OverlapBox(_groundCheckerTrm.position, _groundCheckerSize, 0, _whatIsGround);
        return collider;
    }


    #region velocity control
    public void SetVelocity(float x, float y)
    {
        RbCompo.velocity = new Vector2(x, y);
    }

    public void StopImmediately(bool withYAxis)
    {
        if (withYAxis)
            RbCompo.velocity = Vector2.zero;
        else
            RbCompo.velocity = new Vector2(0, RbCompo.velocity.y);
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (_groundCheckerTrm == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheckerTrm.position, _groundCheckerSize);
        Gizmos.color = Color.white;
    }
}

using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D RbCompo { get; private set; }
    [SerializeField] private Transform _groundCheckerTrm;
    [SerializeField] private Vector2 _groundCheckerSize;
    [SerializeField] private LayerMask _whatIsGround;


    public bool IsGround { get; private set; }
    private float _xMove;
    private Player _player;

    public bool isKnockBack = false;

    public void SetKnockBack(bool value)
    {
        isKnockBack = value;
    }

    public void Initialize(Player player)
    {
        _player = player;
        RbCompo = GetComponent<Rigidbody2D>();
    }

    public void SetXMove(float xMove)
    {
        _xMove = xMove;
    }

    private void FixedUpdate()
    {
        if (isKnockBack) return;

        IsGround = CheckGrounded();
        HorizontalMove();
    }

    private void HorizontalMove()
    {
        float xVelocity = _xMove * _player.Data.moveSpeed;
        RbCompo.velocity = new Vector2(xVelocity, RbCompo.velocity.y);
    }

    public bool CheckGrounded()
    {
        Collider2D collider = Physics2D.OverlapBox(_groundCheckerTrm.position, _groundCheckerSize, 0, _whatIsGround);
        return collider;
    }

    public void Jump()
    {
        StopImmediately(true);
        RbCompo.AddForce( new Vector2(0, _player.Data.jumpPower), ForceMode2D.Impulse);
    }


    public void StopImmediately(bool withYAxis)
    {
        if (withYAxis)
            RbCompo.velocity = Vector2.zero;
        else
            RbCompo.velocity = new Vector2(0, RbCompo.velocity.y);
    }
    

    private void OnDrawGizmosSelected()
    {
        if (_groundCheckerTrm == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheckerTrm.position, _groundCheckerSize);
        Gizmos.color = Color.white;
    }
}

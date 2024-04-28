using System.Collections;
using UnityEngine;

public class PlayerHitState : PlayerState
{
    private float _knockBackTime = 1f; //피격시 1초정지

    public PlayerHitState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Vector2 power = new Vector2(_player.knockBackPower.x, 7f);
        _player.MovementCompo.StopImmediately(true);
        _player.MovementCompo.RbCompo.AddForce(power, ForceMode2D.Impulse);
        _player.MovementCompo.SetKnockBack(true);
        _player.StartCoroutine(DelayKnockback());
    }

    private IEnumerator DelayKnockback()
    {
        yield return new WaitForSeconds(_knockBackTime);
        _player.MovementCompo.SetKnockBack(false); 
        _player.MovementCompo.StopImmediately(false);
        _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }
}

using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.MovementCompo.StopImmediately(false);
    }

    public override void Update()
    {
        base.Update();

        float xMove = _player.PlayerInput.XMove;
        if (Mathf.Abs(xMove) > 0)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Run);
        }
    }
}

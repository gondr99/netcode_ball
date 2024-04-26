using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        float xMove = _player.PlayerInput.XMove;
        _player.MovementCompo.SetXMove(xMove);

        if(Mathf.Approximately( xMove, 0))
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}

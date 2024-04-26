using UnityEngine;

public class PlayerAirState : PlayerState
{
    protected readonly int _yVelocityHash = Animator.StringToHash("YVelocity");
    private int _jumpCount;

    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _jumpCount = _player.Data.jumpCount - 1;//spend one jump already
        _player.PlayerInput.OnJumpKeyEvent += HandleBonusJumpEvent;
    }

    public override void Exit()
    {
        _player.PlayerInput.OnJumpKeyEvent -= HandleBonusJumpEvent;
        base.Exit();
    }

    //additional jump input event handling
    private void HandleBonusJumpEvent()
    {
        if (_jumpCount <= 0) return;

        _jumpCount -= 1;
        //_player.MovementCompo.StopImmediately(true);
        _player.MovementCompo.Jump();
    }

    public override void Update()
    {
        base.Update();
        if (_player.MovementCompo.IsGround)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
            return;
        }

        float yVelocity = _player.MovementCompo.RbCompo.velocity.y;
        _player.AnimCompo.SetFloat(_yVelocityHash, yVelocity);

        float xMove = _player.PlayerInput.XMove;
        _player.MovementCompo.SetXMove(xMove * 0.6f); //입력값의 60퍼만 반영

    }
}

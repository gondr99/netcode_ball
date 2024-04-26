using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundState : PlayerState
{
    protected PlayerGroundState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.OnJumpKeyEvent += HandleJumpKeyEvent;
    }

    public override void Exit()
    {
        _player.PlayerInput.OnJumpKeyEvent -= HandleJumpKeyEvent;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(_player.MovementCompo.IsGround == false)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Air);
        }
    }

    private void HandleJumpKeyEvent()
    {
        if(_player.MovementCompo.IsGround)
        {
            _player.MovementCompo.Jump();
            _stateMachine.ChangeState(PlayerStateEnum.Air);
        }
    }
}

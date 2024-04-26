using UnityEngine;

public abstract class PlayerState
{
    protected Player _player;
    protected PlayerStateMachine _stateMachine;
    
    protected int _animHash;
    protected bool _animationEndTrigger;


    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        _player = player;
        _stateMachine = stateMachine;
        _animHash = Animator.StringToHash(animBoolName);
    }

    public virtual void Enter()
    {
        _player.AnimCompo.SetBool(_animHash, true);
        _animationEndTrigger = false;
    }

    public virtual void Exit()
    {
        _player.AnimCompo.SetBool(_animHash, false);
    }

    public virtual void Update()
    {
        
    }

    public void AnimationEndTrigger()
    {
        _animationEndTrigger = true;
    }
}

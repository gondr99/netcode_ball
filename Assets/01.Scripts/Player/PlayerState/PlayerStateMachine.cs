using System.Collections.Generic;

public class PlayerStateMachine
{
    private Dictionary<PlayerStateEnum, PlayerState> _stateDictionary;
    private Player _player;

    public PlayerState currentState;

    public PlayerStateMachine()
    {
        _stateDictionary = new Dictionary<PlayerStateEnum, PlayerState>();

    }

    public void InitState(PlayerStateEnum stateEnum, Player player)
    {
        _player = player;
        currentState = _stateDictionary[stateEnum];
        currentState.Enter();
    }

    public void ChangeState(PlayerStateEnum newState)
    {
        if (_player.isDead) return;
        if (!_player.canStateChangeable) return;

        currentState.Exit();
        currentState = _stateDictionary[newState];
        currentState.Enter();
    }

    public void AddState(PlayerStateEnum stateEnum, PlayerState state)
    {
        _stateDictionary.Add(stateEnum, state);
    }
}

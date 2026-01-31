using System.Collections.Generic;
public class EnemyStates : States<EnemyCore, EnemyStates>
{
    Dictionary<State, BaseState<EnemyCore, EnemyStates>> _states = new Dictionary<State, BaseState<EnemyCore, EnemyStates>>();
    
    enum State
    {
        Idle, Jump, 
    }
    public EnemyStates(EnemyCore contextCore) : base (contextCore)
    {
        _states[State.Idle] = new EnemyIdleState(Core, this);
        _states[State.Jump] = new EnemyJumpState(Core, this);

    }

    public BaseState<EnemyCore, EnemyStates> Idle => _states[State.Idle];
    public BaseState<EnemyCore, EnemyStates> Jump => _states[State.Jump];

    
}

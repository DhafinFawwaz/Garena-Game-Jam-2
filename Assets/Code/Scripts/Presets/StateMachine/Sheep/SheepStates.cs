using System.Collections.Generic;
public class SheepStates : States<SheepCore, SheepStates>
{
    Dictionary<State, BaseState<SheepCore, SheepStates>> _states = new Dictionary<State, BaseState<SheepCore, SheepStates>>();
    
    enum State
    {
        Alert, Attack, Chase, Death, Eat, Hurt, Idle, Jump, Rush, Wander, 
    }
    public SheepStates(SheepCore contextCore) : base (contextCore)
    {
        _states[State.Alert] = new SheepAlertState(Core, this);
        _states[State.Attack] = new SheepAttackState(Core, this);
        _states[State.Chase] = new SheepChaseState(Core, this);
        _states[State.Death] = new SheepDeathState(Core, this);
        _states[State.Eat] = new SheepEatState(Core, this);
        _states[State.Hurt] = new SheepHurtState(Core, this);
        _states[State.Idle] = new SheepIdleState(Core, this);
        _states[State.Jump] = new SheepJumpState(Core, this);
        _states[State.Rush] = new SheepRushState(Core, this);
        _states[State.Wander] = new SheepWanderState(Core, this);

    }

    public BaseState<SheepCore, SheepStates> Alert => _states[State.Alert];
    public BaseState<SheepCore, SheepStates> Attack => _states[State.Attack];
    public BaseState<SheepCore, SheepStates> Chase => _states[State.Chase];
    public BaseState<SheepCore, SheepStates> Death => _states[State.Death];
    public BaseState<SheepCore, SheepStates> Eat => _states[State.Eat];
    public BaseState<SheepCore, SheepStates> Hurt => _states[State.Hurt];
    public BaseState<SheepCore, SheepStates> Idle => _states[State.Idle];
    public BaseState<SheepCore, SheepStates> Jump => _states[State.Jump];
    public BaseState<SheepCore, SheepStates> Rush => _states[State.Rush];
    public BaseState<SheepCore, SheepStates> Wander => _states[State.Wander];

    
}

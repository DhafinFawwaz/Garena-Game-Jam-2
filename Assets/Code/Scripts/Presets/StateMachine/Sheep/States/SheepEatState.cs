using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SheepEatState : BaseState<SheepCore, SheepStates>
{
    public SheepEatState(SheepCore contextCore, SheepStates States) : base (contextCore, States)
    {
    }

    float _eatTimer = 0f;
    float _eatDuration = 2f;
    float _minEatAmount = 5f;
    float _maxEatAmount = 15f;
    public override void StateEnter()
    {
        _eatTimer = 0f;
        Core.Stats.Eat(Random.Range(_minEatAmount, _maxEatAmount));
    }

    public override void StateUpdate()
    {
        _eatTimer += Time.deltaTime;
        if(_eatTimer >= _eatDuration) {
            SwitchState(States.Wander);
        }   
    }
    public override void StateFixedUpdate()
    {

    }

    public override void StateExit()
    {
        
    }
    public override HitResult OnHurt(HitRequest hitRequest)
    {
        return base.OnHurt(hitRequest);
    }
}

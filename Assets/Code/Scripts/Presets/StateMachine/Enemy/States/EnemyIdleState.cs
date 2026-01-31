using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class EnemyIdleState : BaseState<EnemyCore, EnemyStates>
{
    public EnemyIdleState(EnemyCore contextCore, EnemyStates States) : base (contextCore, States)
    {
    }

    public override void StateEnter()
    {
        Core.OnEnterIdle();
    }

    public override void StateUpdate()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
            SwitchState(States.Jump);
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

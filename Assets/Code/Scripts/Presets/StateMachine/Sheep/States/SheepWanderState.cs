using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SheepWanderState : BaseState<SheepCore, SheepStates>
{
    public SheepWanderState(SheepCore contextCore, SheepStates States) : base (contextCore, States)
    {
    }

    public override void StateEnter()
    {
        
    }

    public override void StateUpdate()
    {
        
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

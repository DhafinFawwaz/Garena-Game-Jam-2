using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SheepChaseState : BaseState<SheepCore, SheepStates>
{
    public SheepChaseState(SheepCore contextCore, SheepStates States) : base (contextCore, States)
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
        if(Core.CurrentSigns.Count == 0) return;
        Core.EntitySkin.IsMoving = true;
        var sign = Core.CurrentSigns[0];
        Vector2 direction = (sign.transform.position - Core.transform.position).normalized;
        direction.y *= 0.5f;
        Core.Rb.linearVelocity = direction * Core.MoveSpeed;
        if(Core.Rb.linearVelocity.sqrMagnitude < 0.01f) {
            Core.EntitySkin.IsMoving = false;
        } else {
            Core.EntitySkin.LookDirection(Core.Rb.linearVelocity);
        }
    }

    public override void StateExit()
    {
        Core.EntitySkin.IsMoving = false;
    }
    public override HitResult OnHurt(HitRequest hitRequest)
    {
        return base.OnHurt(hitRequest);
    }
}

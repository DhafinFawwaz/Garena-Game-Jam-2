using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SheepRushState : BaseState<SheepCore, SheepStates>
{
    public SheepRushState(SheepCore contextCore, SheepStates States) : base (contextCore, States)
    {
    }

    public override void StateEnter()
    {
        Core.Skin.IsMoving = true;
    }

    public override void StateUpdate()
    {
        
    }
    public override void StateFixedUpdate()
    {
        var closestDetectedSheep = Core.Detector.GetClosestSheepCoreWithDifferentTeamID(Core.Stats.TeamID);
        if(closestDetectedSheep != null) {
            Vector2 direction = (closestDetectedSheep.transform.position - Core.transform.position).normalized;
            direction.y *= 0.5f;
            Core.Rb.linearVelocity = direction * Core.MoveSpeed;
            if(Core.Rb.linearVelocity.sqrMagnitude < 0.01f) {
                Core.Skin.IsMoving = false;
            } else {
                Core.Skin.LookDirection(Core.Rb.linearVelocity);
            }

            Core.Skin.LookDirection(Core.Rb.linearVelocity);
            if(Core.Detector.IsCloseEnoughToAttack(closestDetectedSheep)) {
                SwitchState(States.Attack);
            }
        } else {
            SwitchState(States.Wander);
        }
    }

    public override void StateExit()
    {
        Core.Skin.IsMoving = false;
    }
    public override HitResult OnHurt(HitRequest hitRequest)
    {
        return base.OnHurt(hitRequest);
    }
}

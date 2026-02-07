using System;
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

    Func<int, SheepCore> GetClosestSheepCore(SignType type) {
        if(type == SignType.Combat) {
            return Core.Detector.GetClosestSheepCoreWithDifferentTeamID;
        } else if(type == SignType.Cannibal) {
            return Core.Detector.GetClosestSheepCoreWithSameTeamID;
        }
        return null;
    }

    public override void StateFixedUpdate()
    {
        var sign = Core.GetLatestSign();
        var func = GetClosestSheepCore(sign.Type);
        if(func == null) return;

        var closestDetectedSheep = func(Core.Stats.TeamID);
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

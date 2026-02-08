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


    Sign _currentTargetSign = null;
    public override void StateFixedUpdate()
    {
        if(Core.CurrentSigns.Count == 0) return;
        Core.Skin.IsMoving = true;
        var sign = Core.CurrentSigns[Core.CurrentSigns.Count-1];
        if(_currentTargetSign != null && _currentTargetSign != sign) {
            _currentTargetSign = null;
            if(Core.LastSignThatCausedAlert == sign) return;
            Core.LastSignThatCausedAlert = sign;
            SwitchState(States.Alert);
        }
        _currentTargetSign = sign;
        Vector2 direction = (sign.transform.position - Core.transform.position).normalized;
        direction.y *= 0.5f;
        Core.Rb.linearVelocity = direction * Core.MoveSpeed;
        if(Core.Rb.linearVelocity.sqrMagnitude < 0.01f) {
            Core.Skin.IsMoving = false;
        } else {
            Core.Skin.LookDirection(Core.Rb.linearVelocity);
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

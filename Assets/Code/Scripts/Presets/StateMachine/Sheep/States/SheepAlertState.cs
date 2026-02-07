using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SheepAlertState : BaseState<SheepCore, SheepStates>
{
    public SheepAlertState(SheepCore contextCore, SheepStates States) : base (contextCore, States)
    {
    }

    public override void StateEnter()
    {
        _timer = 0f;
        _alertDuration += Random.Range(-0.2f, 0f);
    }

    float _timer = 0f;
    float _alertDuration = 1f;
    float _alertSpeed = 4f;
    public override void StateUpdate()
    {
        _timer += Time.deltaTime;
        if(_timer >= _alertDuration && Core.CurrentSigns.Count > 0) {
            _timer = 0f;
            Core.SwitchState(States.Chase);
        }
        Core.EntitySkin.AlertJump(Mathf.Min(1f, _timer * _alertSpeed));
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

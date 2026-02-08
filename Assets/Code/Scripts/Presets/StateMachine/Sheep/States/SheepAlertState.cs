using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SheepAlertState : BaseState<SheepCore, SheepStates>
{
    public SheepAlertState(SheepCore contextCore, SheepStates States) : base(contextCore, States)
    {
    }

    public override void StateEnter()
    {
        _timer = 0f;
        _alertDuration += UnityEngine.Random.Range(-0.2f, 0f);

        Sign sign1 = null;
        if (Core.CurrentSigns.Count > 0) sign1 = Core.CurrentSigns[Core.CurrentSigns.Count - 1];
        var trust = Core.Stats.CurrentTrust;


        if (sign1.Type == SignType.GoHere && trust >= _minimumTrustToChase)
        {
            Core.VFX.PlayAlertVFX();
        }
        else if (sign1.Type == SignType.GoHere && trust <= _minimumTrustToChase)
        {
            Core.VFX.PlayAlertConfusedVFX();
        }

        if (sign1.Type != SignType.GoHere && trust < _minimumTrustToChase)
        {
            Core.VFX.PlayAlertVFX();
        }
        else if (sign1.Type != SignType.GoHere && trust >= _minimumTrustToChase)
        {
            Core.VFX.PlayAlertConfusedVFX();
        }

    }

    float _timer = 0f;
    float _alertDuration = 1f;
    float _alertSpeed = 4f;
    float _minimumTrustToChase = 40f;
    public override void StateUpdate()
    {
        _timer += Time.deltaTime;
        var trust = Core.Stats.CurrentTrust;
        Sign sign1 = null;
        if (Core.CurrentSigns.Count > 0) sign1 = Core.CurrentSigns[Core.CurrentSigns.Count - 1];

        if (_timer >= _alertDuration && Core.CurrentSigns.Count > 0)
        {
            _timer = 0f;
            handleSignType(sign1, trust,
                () => { Core.SwitchState(States.Chase); },
                () => { Core.SwitchState(States.Wander); },
                () => { Core.SwitchState(States.Rush); }
            );
        }
        Core.Skin.AlertJump(Mathf.Min(1f, _timer * _alertSpeed));

        // handleSignType(sign1, trust);
    }

    void handleSignType(Sign sign1, float trust, Action onChase = null, Action onWander = null, Action onRush = null)
    {
        if (sign1 == null) return;

        if (trust < _minimumTrustToChase)
        {
            if (sign1.Type == SignType.DontGoHere)
            {
                onChase?.Invoke();
            }
            else if (sign1.Type == SignType.NoCombat || sign1.Type == SignType.NoCannibal)
            {
                onRush?.Invoke();
            }
        }
        else
        {
            onWander?.Invoke();
        }

        if (trust >= _minimumTrustToChase)
        {
            if (sign1.Type == SignType.GoHere)
            {
                onChase?.Invoke();
            }
            else
            {
                onWander?.Invoke();
            }
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

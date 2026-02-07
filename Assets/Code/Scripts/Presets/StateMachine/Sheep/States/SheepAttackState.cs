using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class SheepAttackState : BaseState<SheepCore, SheepStates>
{
    public SheepAttackState(SheepCore contextCore, SheepStates States) : base (contextCore, States)
    {
    }

    float _attackCooldown = 0f;
    const float _attackMaxCooldown = 1f;
    public override void StateEnter()
    {
        
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
    Func<int, HitRequest, SheepCore> GetAttackTargetFunc(SignType type) {
        if(type == SignType.Combat) {
            return Core.Attack.HitClosestSheepCoreWithDifferent;
        } else if(type == SignType.Cannibal) {
            return Core.Attack.HitClosestSheepCoreWithSameTeamID;
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
        }


        _attackCooldown += Time.deltaTime;
        if(_attackCooldown >= _attackMaxCooldown) {
            GetAttackTargetFunc(sign.Type)(Core.Stats.TeamID, new HitRequest{
                Damage=Core.Stats.AttackDamage,
                Direction=(Core.Skin.ForwardDirection),
            });
            _attackCooldown = 0f;
        }

    }

    public override void StateExit()
    {
        
    }
    public override HitResult OnHurt(HitRequest hitRequest)
    {
        return base.OnHurt(hitRequest);
    }
}

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
        _attackCooldown += Time.deltaTime;
        if(_attackCooldown >= _attackMaxCooldown) {
            Core.Attack.HitClosestSheepCore(Core.Stats.TeamID, new HitRequest{
                Damage=Core.Stats.AttackDamage,
                Direction=(Core.Skin.ForwardDirection),
            });
            _attackCooldown = 0f;
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

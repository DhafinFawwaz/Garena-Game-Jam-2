using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SheepWanderState : BaseState<SheepCore, SheepStates>
{
    float timeOffset;
    float noiseOffsetX;
    float noiseOffsetY;
    float wanderSpeed = 0.5f;
    
    public SheepWanderState(SheepCore contextCore, SheepStates States) : base (contextCore, States)
    {
        noiseOffsetX = Random.Range(0f, 1000f);
        noiseOffsetY = Random.Range(0f, 1000f);
    }

    float _wanderTimer = 0f;
    float _eatFoodAt = 3f;
    public override void StateEnter()
    {
        timeOffset = 0f;
        Core.Skin.IsMoving = true;
        _wanderTimer = 0f;
    }

    public override void StateUpdate()
    {
        _wanderTimer += Time.deltaTime;
        if(_wanderTimer >= _eatFoodAt) {
            SwitchState(States.Eat);
        }
    }
    public override void StateFixedUpdate()
    {
        timeOffset += Time.fixedDeltaTime;
        
        float noiseX = Mathf.PerlinNoise(noiseOffsetX + timeOffset * 0.5f, 0f) * 2f - 1f;
        float noiseY = Mathf.PerlinNoise(noiseOffsetY + timeOffset * 0.5f, 0f) * 2f - 1f;
        
        Vector2 direction = new Vector2(noiseX, noiseY * 0.5f).normalized;
        Core.Rb.linearVelocity = direction * Core.MoveSpeed * wanderSpeed;
        
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

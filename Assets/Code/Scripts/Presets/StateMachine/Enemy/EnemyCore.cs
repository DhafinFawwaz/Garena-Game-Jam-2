using DhafinFawwaz.AnimationUI;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyCore : Core<EnemyCore, EnemyStates>
{
    [SerializeField] AnimationUI _squishAUI;
    [SerializeField] AnimationUI _thrownAUI;
    [SerializeField] EnemyStats _stats;
    [SerializeField] Healthbar _healthbar;
    [SerializeField] DropSpawner _dropSpawner;
    [SerializeField] Collider2D _hitboxCollider;
    void Awake()
    {
        States = new EnemyStates(this);
        CurrentState = States.Idle;
        CurrentState.StateEnter();
    }

    public override HitResult OnHurt(HitRequest hitRequest)
    {
        if( _stats.CurrentHealth <= 0) return new HitResult { Defeat = true};
        
        _stats.CurrentHealth -= hitRequest.Damage;
        _healthbar.SetHealthNormalized(_stats.CalculateHealthNormalized());
        
        if(_stats.CurrentHealth < 0) {
            _stats.CurrentHealth = 0;
            _thrownAUI.StopAndPlay();
            _dropSpawner.SpawnDrops(transform.position);
            DisableCollider();
            return new HitResult { Defeat = true};
        }
            
        _squishAUI.StopAndPlay();
        return new HitResult();
    }

    void DisableCollider() {
        _hitboxCollider.enabled = false;
    }


    void Update()
    {
        CurrentState.StateUpdate();
        if(Keyboard.current.hKey.wasPressedThisFrame)
        {
            var res = GetComponent<Core>().OnHurt(new HitRequest(
                damage: 10, 
                knockback: 100, 
                direction: Vector2.up, 
                element: Element.Fire, 
                stunDuration: 0.5f,
                hitPosition: transform.position
            ));
        }
    }
    void FixedUpdate()
    {
        CurrentState.StateFixedUpdate();
    }

    public void OnEnterIdle()
    {
        Debug.Log("Enter Idle");
    }

    
}

using UnityEngine;

public class EntityStats : MonoBehaviour
{
    float _currentTrust = 80f;
    float _currentHunger = 80f;
    public float CurrentTrust { get => _currentTrust; set => _currentTrust = Mathf.Clamp(value, 0f, MaxTrust); }
    public float CurrentHunger { get => _currentHunger; set => _currentHunger = Mathf.Clamp(value, 0f, MaxHunger); }
    public EntityType State = EntityType.Neutral;


    public const float MaxTrust = 100f;
    public const float MaxHunger = 100f;
    public const float InitialTrust = 50f;
    public const float InitialHunger = 50f;

    public void InitializeStats() {
        _currentTrust = InitialTrust;
        _currentHunger = InitialHunger;
    }
}

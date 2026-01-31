using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] float _maxHealth = 100f;
    public float MaxHealth { get => _maxHealth; }
    [SerializeField] float _currentHealth = 100f;
    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    public float CalculateHealthNormalized()
    {
        return CurrentHealth / MaxHealth;
    }
}

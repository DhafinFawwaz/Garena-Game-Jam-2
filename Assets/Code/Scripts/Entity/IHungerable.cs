using System;

public interface IHungerable
{
    float HungerLevel { get; }
    float MaxHunger { get; }
    void IncreaseHunger(float amount);
    void DecreaseHunger(float amount);
    event Action OnHungerDepleted;
}

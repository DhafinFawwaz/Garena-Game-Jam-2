public interface ITrustable
{
    float TrustLevel { get; }
    void IncreaseTrust(float amount);
    void DecreaseTrust(float amount);
}

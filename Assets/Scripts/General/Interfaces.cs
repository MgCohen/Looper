

public interface IDamageable
{
    bool dead { get; set; }

    void TakeDamage(int amount);
}
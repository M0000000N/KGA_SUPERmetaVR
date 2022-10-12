public interface IDamageable
{
    public bool IsInteracting { get; }

    public void TakeDamage(UnityEngine.GameObject _attacker);
}
using UnityEngine;

public interface IDamageable
{
    public bool IsInteracting { get; }

    public void TakeDamage(GameObject _attacker);
}
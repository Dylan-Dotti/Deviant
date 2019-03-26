using UnityEngine;

public class PlayerBlasterProjectile : Projectile
{
    private DamageNumberPool dmgNumPool;

    private void Awake()
    {
        dmgNumPool = ObjectPoolManager.Instance.GetDamageNumberPool(
            ObjectPoolManager.DamagerType.Player);
    }

    private void OnEnable()
    {
        ReturnToPoolAfter(2.5f);
    }

    protected override void ApplyDamage(int damage, Health targetHealth)
    {
        if (targetHealth.CurrentHealth != 0)
        {
            DamageNumber dmgNumber = dmgNumPool.Get();
            dmgNumber.DamageText.text = Mathf.Min(targetHealth.
                CurrentHealth, damage).ToString();
            dmgNumber.SpawnAtPos(transform.position + Vector3.up);
        }
        base.ApplyDamage(damage, targetHealth);
    }
}

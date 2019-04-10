using UnityEngine;

public class PlayerSingleLaser : SingleLaser
{
    private DamageNumberPool dmgNumPool;

    protected override void Awake()
    {
        base.Awake();
        dmgNumPool = ObjectPoolManager.Instance.GetDamageNumberPool(
            ObjectPoolManager.DamagerType.Player);
    }

    protected override void ApplyDamage(RaycastHit hitTarget, 
        Health targetHealth, int damage)
    {
        if (targetHealth.CurrentHealth != 0)
        {
            DamageNumber dmgNumber = dmgNumPool.Get();
            dmgNumber.DamageText.text = Mathf.Min(targetHealth.
                CurrentHealth, damage).ToString();
            dmgNumber.SpawnAtPos(hitTarget.point + Vector3.up);
        }
        base.ApplyDamage(hitTarget, targetHealth, damage);
    }
}


public class RangedDroneProjectile : Projectile
{
    private DamageNumberPool dmgNumPool;

    private void Awake()
    {
        dmgNumPool = ObjectPoolManager.Instance.GetDamageNumberPool(
            ObjectPoolManager.DamagerType.RangedDrone);
    }

    protected override void ApplyDamage(int damageAmount, Health targetHealth)
    {
        //damage numbers
        base.ApplyDamage(damageAmount, targetHealth);
    }
}

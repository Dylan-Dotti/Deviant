
public class VortexSpawnerBlaster : SingleBlaster
{
    public override void InitProjectilePool()
    {
        projectilePool = ObjectPoolManager.Instance.GetProjectilePool(
            ObjectPoolManager.ProjectileType.VortexSpawner);
    }
}

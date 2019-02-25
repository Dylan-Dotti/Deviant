
public class PlayerSingleBlaster : SingleBlaster
{
    public override void InitProjectilePool()
    {
        projectilePool = ObjectPoolManager.Instance.GetProjectilePool(
            ObjectPoolManager.ProjectileType.Player);
    }
}

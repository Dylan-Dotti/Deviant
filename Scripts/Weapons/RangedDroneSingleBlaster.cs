
public class RangedDroneSingleBlaster : SingleBlaster
{
    public override void InitProjectilePool()
    {
        projectilePool = ObjectPoolManager.Instance
            .GetProjectilePool(ObjectPoolManager
            .ProjectileType.RangedDrone);
    }


}


public sealed class PlayerProjectilePool : ObjectPool<PlayerBlasterProjectile>
{
    public static PlayerProjectilePool Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
    }
}

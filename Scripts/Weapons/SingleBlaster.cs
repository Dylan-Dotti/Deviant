using UnityEngine;

public abstract class SingleBlaster : BlasterWeapon
{
    public override IntRange ProjectileDmgRange
    {
        get => projectileDamageRange;
        set => projectileDamageRange = value;
    }

    [SerializeField]
    protected Transform firePoint;
    [SerializeField]
    protected ParticleSystem fireParticles;

    [SerializeField]
    private IntRange projectileDamageRange = new IntRange(1, 1);

    protected ProjectilePool projectilePool;

    private AudioSource fireSound;

    protected virtual void Awake()
    {
        fireSound = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        InitProjectilePool();
    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        Projectile projectile = projectilePool.Get();
        projectile.DamageRange = ProjectileDmgRange;
        Rigidbody projectileRBody = projectile.GetComponent<Rigidbody>();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;
        projectile.gameObject.SetActive(true);
        if (fireSound != null)
        {
            fireSound?.PlayOneShot(fireSound.clip);
        }
        if (fireParticles != null)
        {
            fireParticles.Play();
        }
    }

    public abstract void InitProjectilePool();
}

using UnityEngine;

public abstract class SingleBlaster : Weapon
{
    [SerializeField]
    protected Transform firePoint;
    [SerializeField]
    protected ParticleSystem fireParticles;

    protected ProjectilePool projectilePool;

    private WeaponRecoil recoiler;
    private AudioSource fireSound;

    private void Awake()
    {
        recoiler = GetComponent<WeaponRecoil>();
        fireSound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        InitProjectilePool();
    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        Projectile projectile = projectilePool.Get();
        Rigidbody projectileRBody = projectile.GetComponent<Rigidbody>();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;
        projectile.gameObject.SetActive(true);
        recoiler.AttemptRecoil();
        fireSound?.PlayOneShot(fireSound.clip);
        fireParticles.Play();
    }

    public abstract void InitProjectilePool();
}


public abstract class LaserWeapon : Weapon
{
    public bool IsFiring { get; protected set; } = false;

    protected override void Update()
    {

    }

    public override void FireWeapon()
    {
        IsFiring = true;
    }

    public virtual void CancelFireWeapon()
    {
        IsFiring = false;
    }
}

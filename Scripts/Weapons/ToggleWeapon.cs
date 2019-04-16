
/* Weapon that can be toggled on and off.
 * Currently only used for laser weapons
 */
public abstract class ToggleWeapon : Weapon
{
    public bool IsFiring { get; protected set; } = false;

    public override void FireWeapon()
    {
        base.FireWeapon();
        IsFiring = true;
    }

    public override void AttemptFireWeapon()
    {
        if (!IsFiring)
        {
            base.AttemptFireWeapon();
        }
    }

    public override void CancelFireWeapon()
    {
        IsFiring = false;
    }
}

using UnityEngine;

/* Superclass for all weapons
 */
public abstract class Weapon : MonoBehaviour
{
    public delegate void WeaponDelegate();

    public event WeaponDelegate WeaponFiredEvent;

    public float TimeSinceLastFire { get; private set; }
    public float FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }

    [SerializeField]
    private float fireRate;

    private Quaternion originalOrientation;

    protected virtual void Start()
    {
        originalOrientation = transform.localRotation;
    }

    protected virtual void Update()
    {
        TimeSinceLastFire += Time.deltaTime;
    }

    public virtual void AttemptFireWeapon()
    {
        if (TimeSinceLastFire >= FireRate && enabled)
        {
            FireWeapon();
        }
    }

    public virtual void FireWeapon()
    {
        InvokeWeaponFiredEvent();
        TimeSinceLastFire = 0;
    }

    // Not used by most weapons, but needed for generality
    public virtual void CancelFireWeapon()
    {

    }

    public virtual void TurnToFace(Vector3 targetPos)
    {
        transform.LookAt(targetPos);
    }

    public virtual void ResetOrientation()
    {
        transform.localRotation = originalOrientation;
    }

    // Event needs to be placed in a function to be called from subclass
    protected void InvokeWeaponFiredEvent()
    {
        WeaponFiredEvent?.Invoke();
    }
}

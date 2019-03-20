using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public delegate void WeaponDelegate();

    public event WeaponDelegate WeaponFiredEvent;

    //public virtual bool WeaponEnabled { get; set; } = true;
    public float TimeSinceLastFire { get; private set; }
    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
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
        WeaponFiredEvent?.Invoke();
        TimeSinceLastFire = 0;
    }

    public virtual void TurnToFace(Vector3 targetPos)
    {
        transform.LookAt(targetPos);
    }

    public virtual void ResetOrientation()
    {
        transform.rotation = originalOrientation;
    }
}

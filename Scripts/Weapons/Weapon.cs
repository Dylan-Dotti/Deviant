using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float TimeSinceLastFire { get; private set; }
    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }

    [SerializeField]
    private float fireRate;

    protected virtual void Update()
    {
        TimeSinceLastFire += Time.deltaTime;
    }

    public virtual void AttemptFireWeapon()
    {
        if (TimeSinceLastFire >= FireRate)
        {
            FireWeapon();
        }
    }

    public virtual void FireWeapon()
    {
        TimeSinceLastFire = 0;
    }

    public virtual void TurnToFace(Vector3 targetPos)
    {
        transform.LookAt(targetPos);
    }
}

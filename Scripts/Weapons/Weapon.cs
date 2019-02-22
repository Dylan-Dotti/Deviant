using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float TimeSinceLastFire { get; protected set; }
    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }

    [SerializeField]
    private float fireRate;

    //[SerializeField]
    protected AudioSource fireSound;

    protected virtual void Awake()
    {
        fireSound = GetComponent<AudioSource>();
    }

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

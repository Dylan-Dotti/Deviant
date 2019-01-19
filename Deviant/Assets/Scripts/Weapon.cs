using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    [SerializeField]
    protected Projectile projectilePrefab;

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

    public abstract void FireWeapon();

    public abstract void TurnToFace(Vector3 targetPos);
}

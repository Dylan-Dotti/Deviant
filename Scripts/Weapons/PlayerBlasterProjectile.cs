using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlasterProjectile : Projectile
{
    [SerializeField]
    private DamageNumberPool damageNumberPool;

    private Rigidbody rBody;
    private Vector3 playerLaunchVelocity;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ReturnToPoolAfter(2.5f);
        playerLaunchVelocity = PlayerCharacter.Instance
            .Controller.TotalLocalVelocity;
    }

    protected override void Update()
    {
        base.Update();
        //transform.Translate((Vector3.forward * MoveSpeed + 
          //  playerLaunchVelocity) * Time.deltaTime);
        //playerVelocity = new Vector3(playerVelocity.x, 0, playerVelocity.z);
        //transform.Translate((Vector3.forward * MoveSpeed /*+ playerVelocity*/) * Time.deltaTime);
        //rBody.velocity = transform.right * MoveSpeed;
    }

    public override void ReturnToPool()
    {
        PlayerProjectilePool.Instance.ReturnToPool(this);
    }

    protected override void ApplyDamage(Health targetHealth)
    {
        if (targetHealth.CurrentHealth != 0)
        {
            //DamageNumber dmgNum = damageNumberPool.Get()
        }
        base.ApplyDamage(targetHealth);
    }
}

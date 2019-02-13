using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlasterProjectile : Projectile
{
    private Rigidbody rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ReturnToPoolAfter(2.5f);
        Vector3 playerVelocity = PlayerCharacter.Instance.Controller.TotalVelocity;
    }

    protected override void Update()
    {
        base.Update();
        //Vector3 playerVelocity = PlayerCharacter.Instance.Controller.TotalVelocity;
        //playerVelocity = new Vector3(playerVelocity.x, 0, playerVelocity.z);
        //transform.Translate((Vector3.forward * MoveSpeed /*+ playerVelocity*/) * Time.deltaTime);
        //rBody.velocity = transform.right * MoveSpeed;
    }
    public override void ReturnToPool()
    {
        PlayerProjectilePool.Instance.ReturnToPool(this);
    }

}

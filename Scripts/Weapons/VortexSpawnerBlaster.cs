using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexSpawnerBlaster : Weapon
{
    [SerializeField]
    private float attackRange;
    [SerializeField]
    Projectile projectilePrefab;

    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        playerTransform = PlayerCharacter.Instance.transform;
    }

    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(transform.position, 
            playerTransform.position) <= attackRange)
        {
            AttemptFireWeapon();
        }
    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        Projectile projectile = Instantiate(projectilePrefab,
            transform.position, Quaternion.identity);
        Vector3 targetPos = new Vector3(playerTransform.position.x,
            transform.position.y, playerTransform.position.z);
        projectile.transform.LookAt(targetPos);
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        Vector3 adjustedPos = new Vector3(targetPos.x,
            transform.position.y, targetPos.z);
        transform.LookAt(targetPos);
    }
}

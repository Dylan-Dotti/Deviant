using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlasterProjectile : Projectile
{
    private void OnEnable()
    {
        ReturnToPoolAfter(3f);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "PlayerBody")
        {
            Character hitTarget = other.GetComponentInParent<Character>();
            if (hitTarget != null)
            {
                ApplyDamage(hitTarget.CharacterHealth);
            }
            StopAllCoroutines();
            ReturnToPool();
        }
    }

    public override void ReturnToPool()
    {
        PlayerProjectilePool.Instance.ReturnToPool(this);
    }
}

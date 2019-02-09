using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IPoolable
{
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public float DamageAmount
    {
        get { return damageAmount; }
        set { damageAmount = value; }
    }

    [SerializeField]
    protected List<string> collidableTags;

    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float damageAmount = 5f;

    public abstract void ReturnToPool();

    public void ReturnToPoolAfter(float delay)
    {
        StartCoroutine("ReturnToPoolDelayed", delay);
    }

    public void CancelReturnToPool()
    {
        StopAllCoroutines();
    }

    protected virtual void ApplyDamage(Health targetHealth)
    {
        targetHealth.CurrentHealth -= damageAmount;
    }

    private IEnumerator ReturnToPoolDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }
}

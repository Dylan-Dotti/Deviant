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
    public int DamageAmount
    {
        get { return damageAmount; }
        set { damageAmount = value; }
    }

    [SerializeField]
    protected List<string> collidableTags;

    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private int damageAmount = 5;

    [SerializeField]
    private GameObject projectileBody;
    [SerializeField]
    private ParticleSystem hitParticles;

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime, Space.Self);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (collidableTags.Contains(other.tag))
        {
            Character hitTarget = other.GetComponentInParent<Character>();
            if (hitTarget != null)
            {
                ApplyDamage(hitTarget.CharacterHealth);
            }
            Vector3 hitPos = other.ClosestPointOnBounds(transform.position);
            CancelReturnToPool();
            if (hitParticles != null)
            {
                StartCoroutine(Explode(hitPos));
            }
        }
    }

    public abstract void ReturnToPool();

    public void ReturnToPoolAfter(float delay)
    {
        StartCoroutine(ReturnToPoolDelayed(delay));
    }

    public void CancelReturnToPool()
    {
        StopAllCoroutines();
    }

    protected virtual void ApplyDamage(Health targetHealth)
    {
        targetHealth.CurrentHealth -= damageAmount;
    }

    protected IEnumerator Explode(Vector3 hitPosition)
    {
        enabled = false;
        projectileBody.SetActive(false);
        hitParticles.transform.position = hitPosition;
        hitParticles.Play();
        yield return new WaitForSeconds(hitParticles.main.duration + 0.1f);
        projectileBody.SetActive(true);
        enabled = true;
        ReturnToPool();
    }

    private IEnumerator ReturnToPoolDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IPoolable<Projectile>
{
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public IntRange DamageRange
    {
        get { return damageRange; }
        set { damageRange = value; }
    }

    public ObjectPool<Projectile> Pool { get; set; }

    [SerializeField]
    protected List<string> collidableTags;

    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private IntRange damageRange = new IntRange(1, 1);

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
                int damage = Random.Range(DamageRange.Min,
                    DamageRange.Max + 1);
                ApplyDamage(damage, hitTarget.CharacterHealth);
            }
            CancelReturnToPool();
            Vector3 hitPos = other.ClosestPointOnBounds(transform.position);
            if (hitParticles != null)
            {
                StartCoroutine(Explode(hitPos));
            }
        }
    }

    public virtual void ReturnToPool()
    {
        Pool.ReturnToPool(this);
    }

    public void ReturnToPoolAfter(float delay)
    {
        StartCoroutine(ReturnToPoolDelayed(delay));
    }

    public void CancelReturnToPool()
    {
        StopAllCoroutines();
    }

    protected virtual void ApplyDamage(int damageAmount, Health targetHealth)
    {
        int damage = Random.Range(DamageRange.Min, 
            DamageRange.Max + 1);
        targetHealth.CurrentHealth -= damage;
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

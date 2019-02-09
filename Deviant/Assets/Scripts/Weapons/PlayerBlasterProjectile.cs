using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlasterProjectile : Projectile
{
    [SerializeField]
    private GameObject projectileBody;
    [SerializeField]
    private ParticleSystem trailParticles;
    [SerializeField]
    private ParticleSystem hitParticles;

    private Rigidbody rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ReturnToPoolAfter(2.5f);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
        //rBody.velocity = transform.right * MoveSpeed;
    }

    private void OnTriggerEnter(Collider other)
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
            StartCoroutine(Explode(hitPos));
        }
    }

    public override void ReturnToPool()
    {
        PlayerProjectilePool.Instance.ReturnToPool(this);
    }


    private IEnumerator Explode(Vector3 hitPosition)
    {
        hitParticles.transform.position = hitPosition;
        hitParticles.Play();
        enabled = false;
        projectileBody.SetActive(false);
        yield return new WaitForSeconds(hitParticles.main.duration);
        projectileBody.SetActive(true);
        enabled = true;
        ReturnToPool();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SingleLaser : ToggleWeapon
{
    public Transform FirePoint => firePoint;

    public float LaserStartWidth
    {
        get => laser.startWidth;
        set => laser.startWidth = value;
    }
    public float LaserEndWidth
    {
        get => laser.endWidth;
        set => laser.endWidth = value;
    }

    public IntRange DamagePerTick
    {
        get => damagePerTick;
        set => damagePerTick = value;
    }

    [SerializeField]
    private IntRange damagePerTick = new IntRange(1, 1);
    [SerializeField]
    private float damageTickInterval = 0.5f;
    [SerializeField]
    private float particleTickInterval = 0.1f;
    [SerializeField]
    private LineRenderer laser;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private ParticleSystem fireParticles;
    [SerializeField]
    private ParticleSystem hitParticles;
    [SerializeField]
    private List<string> collidableTags;

    private Vector3 targetPosition;
    private float timeSinceLastDamage;
    private float timeSinceLastParticles;

    protected override void Update()
    {
        base.Update();
        timeSinceLastDamage += Time.deltaTime;
        timeSinceLastParticles += Time.deltaTime;
        if (laser.enabled)
        {
            if (timeSinceLastDamage >= damageTickInterval)
            {
                ApplyDamage();
            }
            if (timeSinceLastParticles >= particleTickInterval &&
                hitParticles != null)
            {
                RaycastHit closestHit = GetClosestHit();
                hitParticles.transform.position = closestHit.point;
                hitParticles.transform.parent = closestHit.transform;
                hitParticles.Play();
                timeSinceLastParticles = 0;
            }
        }
    }

    private void OnDestroy()
    {
        if (hitParticles != null)
        {
            Destroy(hitParticles.gameObject);
        }
    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        laser.enabled = true;
        if (fireParticles != null)
        {
            fireParticles.Play();
        }
    }

    public void FireAndLerp(float startWidth, float endWidth, float maxDuration)
    {
        LaserStartWidth = startWidth;
        LaserEndWidth = startWidth;
        FireWeapon();
        LerpWidthOverAdjustedDuration(startWidth, endWidth, maxDuration);
    }

    public override void CancelFireWeapon()
    {
        base.CancelFireWeapon();
        laser.enabled = false;
        if (fireParticles != null)
        {
            fireParticles.Stop();
        }
    }

    public void CancelAndLerp(float lerpEndWidth, float lerpDuration)
    {
        StartCoroutine(LerpWidthAndCancelCR(lerpEndWidth, lerpDuration));
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        base.TurnToFace(targetPos);
        targetPosition = targetPos;
        float segmentLength = GetLaserLength() / (laser.positionCount - 1);
        for (int i = 0; i < laser.positionCount; i++)
        {
            laser.SetPosition(i, new Vector3(0, 0, segmentLength * i));
        }
    }

    private float GetLaserLength()
    {
        return GetForwardRaycastHits().Min(h => Vector3.Distance(
            FirePoint.transform.position, h.point)) + 0.1f;
    }

    public void LerpWidthOverDuration(float endWidth, float duration)
    {
        StartCoroutine(LerpWidth(endWidth, duration));
    }

    public float LerpWidthOverAdjustedDuration(float startWidth, 
        float endWidth, float maxDuration)
    {
        float currentWidth = laser.startWidth;
        float lerpDuration = maxDuration * (endWidth - currentWidth) /
            (endWidth - startWidth);
        StartCoroutine(LerpWidth(endWidth, lerpDuration));
        return lerpDuration;
    }

    private IEnumerable<RaycastHit> GetForwardRaycastHits()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        float targetDist = Vector3.Distance(firePoint.position, targetPosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, targetDist);
        return hits.Where(h => collidableTags.Contains(h.collider.tag));
    }

    private RaycastHit GetClosestHit()
    {
        IEnumerable<RaycastHit> hits = GetForwardRaycastHits();
        RaycastHit closestHit = new RaycastHit();
        float closestDist = Mathf.Infinity;
        foreach (RaycastHit rayHit in hits)
        {
            float hitDist = Vector3.Distance(FirePoint.position, rayHit.point);
            if (hitDist < closestDist)
            {
                closestDist = hitDist;
                closestHit = rayHit;
            }
        }
        return closestHit;
    }

    private void ApplyDamage()
    {
        Health targetHealth = GetClosestHit().transform.root.
            GetComponentInChildren<Health>();
        if (targetHealth != null)
        {
            targetHealth.CurrentHealth -= damagePerTick.RandomRangeValue;
            timeSinceLastDamage = 0;
        }
    }

    private IEnumerator LerpWidth(float lerpEndWidth, float duration)
    {
        float startWidth = laser.startWidth;
        float lerpStartTime = Time.time;
        for (float currDuration = 0; currDuration < duration; 
             currDuration = Time.time - lerpStartTime)
        {
            float lerpPercentage = currDuration / duration;
            float lerpWidth = Mathf.Lerp(startWidth, lerpEndWidth, lerpPercentage);
            laser.startWidth = lerpWidth;
            laser.endWidth = lerpWidth;
            yield return null;
        }
        laser.startWidth = laser.endWidth = lerpEndWidth;
    }

    private IEnumerator LerpWidthAndCancelCR(float lerpEndWidth, float duration)
    {
        duration = LerpWidthOverAdjustedDuration(LaserStartWidth,
            lerpEndWidth, duration);
        yield return new WaitForSeconds(duration);
        CancelFireWeapon();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class VortexSpawnerProjectile : Projectile
{
    [SerializeField]
    private ParticleSystemRenderer hitParticlesRenderer;
    [SerializeField]
    private ParticleSystemRenderer particleRenderer;

    [SerializeField]
    private List<Material> possibleSpawnMaterials;

    private LerpRotationToTarget rotator;
    private Transform playerTransform;

    private void Awake()
    {
        rotator = GetComponent<LerpRotationToTarget>();
        playerTransform = PlayerCharacter.Instance.transform;
    }

    private void OnEnable()
    {
        particleRenderer.material = possibleSpawnMaterials[
            Random.Range(0, possibleSpawnMaterials.Count)];
    }

    protected override void Update()
    {
        rotator.TargetPosition = playerTransform.position;
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        hitParticlesRenderer.material = particleRenderer.material;
        base.OnTriggerEnter(other);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerDissipate : AnimationSequence
{
    private List<ParticleSystem> particleEffects;

    private void Awake()
    {
        particleEffects = new List<ParticleSystem>(
            GetComponentsInChildren<ParticleSystem>());
    }

    protected override IEnumerator AnimationSequenceCR()
    {
        foreach (ParticleSystem particles in particleEffects)
        {
            particles.Stop();
        }
        GetComponent<PlayerMagnet>().enabled = false;
        GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        for (int i = 0; i < particleEffects.Count; i++)
        {
            if (particleEffects[i].isPlaying)
            {
                i = -1;
                yield return null;
                continue;
            }
        }
        Destroy(gameObject);
    }
}

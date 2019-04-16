using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexDeathSequence : AnimationSequence
{
    [SerializeField]
    private List<ParticleSystem> particlesToStop;
    [SerializeField]
    private HealthBar healthBar;

    protected override IEnumerator AnimationSequenceCR()
    {
        healthBar.gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        foreach (ParticleSystem particles in particlesToStop)
        {
            particles.Stop();
        }

        //fade out magnet strength
        PlayerMagnet magnet = GetComponent<PlayerMagnet>();
        float startMagnitude = magnet.ForceMagnitude;
        float lerpDuration = particlesToStop[0].main.startLifetime.constant /
            particlesToStop[0].main.simulationSpeed * 0.9f;
        float lerpStartTime = Time.time;
        while (Time.time - lerpStartTime < lerpDuration)
        {
            float lerpPercentage = (Time.time - lerpStartTime) / lerpDuration;
            magnet.ForceMagnitude = Mathf.Lerp(startMagnitude, 0f, lerpPercentage);
            yield return null;
        }
        magnet.ForceMagnitude = 0f;
        magnet.enabled = false;

        bool done;
        do
        {
            done = true;
            foreach (ParticleSystem particles in particlesToStop)
            {
                if (particles.particleCount > 0)
                {
                    done = false;
                    break;
                }
            }
            yield return null;
        }
        while (!done);
        Destroy(gameObject);
    }
}

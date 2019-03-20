using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedEffect : MonoBehaviour
{
    [System.Serializable]
    private class Effect
    {
        public ParticleSystem particles;
        [Range(0, 1)]
        public float healthPercentThreshold;
    }

    [SerializeField]
    private List<Effect> effects;
    [SerializeField]
    private Health monitoredHealth;

    private void OnEnable()
    {
        monitoredHealth.HealthChangedEvent += OnHealthChanged;
        foreach (Effect effect in effects)
        {
            if (monitoredHealth.HealthPercentage <= effect.healthPercentThreshold &&
                !effect.particles.isPlaying)
            {
                effect.particles.Play();
            }
        }
    }

    private void OnDisable()
    {
        monitoredHealth.HealthChangedEvent -= OnHealthChanged;
        foreach (Effect effect in effects)
        {
            effect.particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private void OnHealthChanged(float prevHealth, float newHealth)
    {
        foreach (Effect effect in effects)
        {
            if (monitoredHealth.HealthPercentage <= effect.healthPercentThreshold &&
                !effect.particles.isPlaying)
            {
                effect.particles.Play();
            }
            else if (monitoredHealth.HealthPercentage > effect.healthPercentThreshold &&
                effect.particles.isPlaying)
            {
                effect.particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
}

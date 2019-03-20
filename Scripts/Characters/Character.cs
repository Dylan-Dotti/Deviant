using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Character : MonoBehaviour
{
    public delegate void CharacterDelegate(Character character);

    public Health CharacterHealth { get; private set; }

    protected virtual void Awake()
    {
        CharacterHealth = GetComponent<Health>();
        CharacterHealth.HealthReachedZeroEvent += OnHealthReachedZero;
    }

    protected virtual void OnHealthReachedZero(float prevHealth, float newHealth)
    {
        Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public Coroutine LerpScaleOverDuration(List<Transform> lerpObjects, float newScale, float duration)
    {
        return StartCoroutine(LerpScale(lerpObjects, newScale, duration));
    }

    private IEnumerator LerpScale(List<Transform> lerpObjects, float newScale, float duration)
    {
        Dictionary<Transform, Vector3> originalScales = lerpObjects.ToDictionary(
            t => t, t => new Vector3(t.localScale.x, t.localScale.y, t.localScale.z));

        float shrinkStartTime = Time.time;
        while (Time.time - shrinkStartTime < duration)
        {
            float lerpPercentage = Mathf.Min(Time.time - shrinkStartTime, 
                duration) / duration;
            foreach (Transform childTransform in lerpObjects)
            {
                Vector3 origScale = originalScales[childTransform];
                childTransform.transform.localScale = Vector3.Lerp(origScale, 
                    origScale * newScale, lerpPercentage);
            }
            yield return null;
        }
    }
}

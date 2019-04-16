using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Character is the superclass of all enemies and the player */
[RequireComponent(typeof(Health))]
public abstract class Character : MonoBehaviour
{
    public Health CharacterHealth { get; private set; }

    protected virtual void Awake()
    {
        CharacterHealth = GetComponent<Health>();
        CharacterHealth.HealthChangedEvent += OnHealthChanged;
    }

    /* Called when CharacterHealth.CurrentHealth changes */
    protected virtual void OnHealthChanged(float prevHealth, float newHealth)
    {
        if (prevHealth != 0 && newHealth == 0)
        {
            Die();
        }
    }

    public abstract void Die();

    public Coroutine LerpScaleOverDuration(IEnumerable<Transform> lerpObjects, float newScale, float duration)
    {
        return StartCoroutine(LerpScale(lerpObjects, newScale, duration));
    }

    /* Lerps the scale of the specified objects to newScale over duration.
     * Currently it can lerp any objects, not just ones belonging to the character,
     * mostly for reusability purposes. Not sure how to fix yet
     */
    private IEnumerator LerpScale(IEnumerable<Transform> lerpObjects, float newScale, float duration)
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

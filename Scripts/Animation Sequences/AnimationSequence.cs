using System.Collections;
using UnityEngine;

/* Superclass for all complex animations.
 * Mostly used for death animations and some spawn animations.
 */
public abstract class AnimationSequence : MonoBehaviour
{
    public Coroutine PlayAnimation()
    {
        return StartCoroutine(AnimationSequenceCR());
    }

    public virtual void CancelSequence()
    {
        StopAllCoroutines();
    }

    protected abstract IEnumerator AnimationSequenceCR();
}

using System.Collections;
using UnityEngine;

public abstract class AnimationSequence : MonoBehaviour
{
    public bool IsPlaying { get; protected set; } = false;

    public Coroutine PlayAnimation()
    {
        if (!IsPlaying)
        {
            return StartCoroutine(AnimationSequenceCR());
        }
        return null;
    }

    public virtual void CancelSequence()
    {
        StopAllCoroutines();
    }

    protected abstract IEnumerator AnimationSequenceCR();
}

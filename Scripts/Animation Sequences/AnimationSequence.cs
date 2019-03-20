using System.Collections;
using UnityEngine;

public abstract class AnimationSequence : MonoBehaviour
{
    public bool IsPlaying { get; protected set; } = false;

    public virtual void PlayAnimation()
    {
        if (!IsPlaying)
        {
            StartCoroutine(PlayAnimationSequence());
        }
    }

    public virtual void CancelSequence()
    {
        StopAllCoroutines();
    }

    protected abstract IEnumerator PlayAnimationSequence();
}

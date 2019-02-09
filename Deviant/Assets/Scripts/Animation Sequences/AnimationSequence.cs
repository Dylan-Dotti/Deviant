using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationSequence : MonoBehaviour
{
    public bool IsPlaying { get; protected set; } = false;

    public void PlayAnimation()
    {
        StartCoroutine(PlayAnimationSequence());
    }

    public abstract void CancelSequence();

    protected abstract IEnumerator PlayAnimationSequence();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionPanel : MonoBehaviour
{
    private ImageColorLerp alphaLerper;

    private void Awake()
    {
        alphaLerper = GetComponent<ImageColorLerp>();
        DontDestroyOnLoad(gameObject);
    }

    public Coroutine FadeIn()
    {
        return alphaLerper.LerpForward();
    }

    public Coroutine FadeOut()
    {
        return alphaLerper.LerpReverse();
    }
}

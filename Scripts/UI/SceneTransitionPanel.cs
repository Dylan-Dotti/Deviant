using UnityEngine;

[RequireComponent(typeof(ImageColorLerp))]
public class SceneTransitionPanel : MonoBehaviour
{
    private ImageColorLerp alphaLerper;

    private void Awake()
    {
        alphaLerper = GetComponent<ImageColorLerp>();
    }

    public Coroutine FadeForward()
    {
        return alphaLerper.LerpForward();
    }

    public Coroutine FadeReverse()
    {
        return alphaLerper.LerpReverse();
    }
}

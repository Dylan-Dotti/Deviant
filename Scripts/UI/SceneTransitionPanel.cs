using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ImageColorLerp))]
public class SceneTransitionPanel : MonoBehaviour
{
    private ImageColorLerp alphaLerper;

    private void Awake()
    {
        alphaLerper = GetComponent<ImageColorLerp>();
    }

    public void TransitionToScene(int sceneIndex)
    {
        gameObject.SetActive(true);
        StartCoroutine(TransitionToSceneCR(sceneIndex));
    }

    public void TransitionToNextScene()
    {
        TransitionToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void TransitionToPrevScene()
    {
        TransitionToScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void TransitionToReloadScene()
    {
        TransitionToScene(SceneManager.GetActiveScene().buildIndex);
    }

    public Coroutine FadeTransparent()
    {
        return alphaLerper.LerpForward();
    }

    public Coroutine FadeOpaque()
    {
        return alphaLerper.LerpReverse();
    }

    private IEnumerator TransitionToSceneCR(int sceneIndex)
    {
        yield return FadeOpaque();
        SceneManager.LoadScene(sceneIndex);
    }
}

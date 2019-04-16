using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionPanel : MonoBehaviour
{
    public static SceneTransitionPanel Instance { get; private set; }

    [SerializeField]
    private float transitionDuration = 3f;

    private Image panelImage;
    private ImageColorLerp alphaLerper;

    private void Awake()
    {
        if (Instance == null)
        {
            Time.timeScale = 1;
            Instance = this;
            panelImage = GetComponent<Image>();
            alphaLerper = GetComponent<ImageColorLerp>();
        }
    }

    public void TransitionToScene(int sceneIndex)
    {
        panelImage.enabled = true;
        StartCoroutine(TransitionToSceneCR(sceneIndex, transitionDuration));
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
        panelImage.enabled = true;
        return StartCoroutine(FadeAlphaCR(transitionDuration, false));
    }

    public Coroutine FadeOpaque()
    {
        panelImage.enabled = true;
        return StartCoroutine(FadeAlphaCR(transitionDuration, true));
    }

    private IEnumerator TransitionToSceneCR(int sceneIndex, float duration)
    {
        yield return FadeOpaque();
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator FadeAlphaCR(float duration, bool opaque)
    {
        float startAlpha = opaque ? 0 : 1;
        float endAlpha = opaque ? 1 : 0;
        float lerpStartTime = Time.unscaledTime;
        for (float elapsed = 0; elapsed < duration;
             elapsed = Time.unscaledTime - lerpStartTime)
        {
            float lerpPercent = elapsed / duration;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, lerpPercent);
            panelImage.color = new Color(panelImage.color.r, panelImage.color.g,
                panelImage.color.b, newAlpha);
            yield return new WaitForSecondsRealtime(0.02f);
        }
        panelImage.color = new Color(panelImage.color.r, panelImage.color.g,
            panelImage.color.b, endAlpha);
        if (!opaque)
        {
            panelImage.enabled = false;
        }
    }
}

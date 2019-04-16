using System.Collections;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private SceneTransitionPanel transitionPanel;

    private RecursiveUIAlphaLerper alphaLerper;

    private void Awake()
    {
        alphaLerper = GetComponent<RecursiveUIAlphaLerper>();
        PlayerCharacter.Instance.PlayerDeathEvent += 
            () => StartCoroutine(DelayedActivation());
    }

    private void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void StartNewGame()
    {
        enabled = false;
        transitionPanel.TransitionToReloadScene();
    }

    public void TransitionMainMenu()
    {
        enabled = false;
        transitionPanel.TransitionToPrevScene();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(4);
        enabled = true;
    }
}

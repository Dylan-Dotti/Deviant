using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public static StartMenu Instance { get; private set; }

    [SerializeField]
    private SceneTransitionPanel transitionPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(InitCR());
    }

    public void StartGame()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator InitCR()
    {
        yield return transitionPanel.FadeIn();
        transitionPanel.gameObject.SetActive(false);
    }

    private IEnumerator LoadGame()
    {
        transitionPanel.gameObject.SetActive(true);
        yield return transitionPanel.FadeOut();
        SceneManager.LoadSceneAsync(1);
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu Instance { get; private set; }

    public bool IsPaused { get; private set; }

    public UnityAction GamePausedEvent;
    public UnityAction GameResumedEvent;

    [SerializeField]
    private List<Button> menuButtons;
    [SerializeField]
    private GameObject pauseMenuUI;
    [SerializeField]
    private GameObject quitConfirmPanel;
    [SerializeField]
    private GameObject exitConfirmPanel;

    private PlayerController pController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        pController = PlayerCharacter.Instance.Controller;
        GamePausedEvent?.Invoke();
        Time.timeScale = 0;
        pController.enabled = false;
        pauseMenuUI.SetActive(true);
    }

    private void OnDisable()
    {
        pauseMenuUI.SetActive(false);
        pController.enabled = true;
        Time.timeScale = 1;
        GameResumedEvent?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        enabled = true;
    }

    public void ResumeGame()
    {
        enabled = false;
    }

    public void OpenOptionsMenu()
    {

    }

    public void QuitToMainMenu()
    {
        SceneTransitionPanel.Instance.TransitionToPrevScene();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenQuitConfirmPanel(bool exitGame)
    {
        if (exitGame)
        {
            exitConfirmPanel.gameObject.SetActive(true);
        }
        else
        {
            quitConfirmPanel.gameObject.SetActive(true);
        }
        SetButtonsInteractable(false);
    }

    public void CloseQuitConfirmPanel(bool exitGame)
    {
        if (exitGame)
        {
            exitConfirmPanel.gameObject.SetActive(false);
        }
        else
        {
            quitConfirmPanel.gameObject.SetActive(false);
        }
        SetButtonsInteractable(true);
    }

    private void SetButtonsInteractable(bool interactable)
    {
        menuButtons.ForEach(b => b.interactable = interactable);
    }
}

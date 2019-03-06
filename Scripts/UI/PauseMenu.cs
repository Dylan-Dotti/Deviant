using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public delegate void PauseMenuDelegate();

    public static PauseMenu Instance { get; private set; }

    public bool IsPaused { get; private set; }

    public static PauseMenuDelegate GamePausedEvent;
    public static PauseMenuDelegate GameResumedEvent;

    [SerializeField]
    private GameObject pauseMenuUI;

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
}

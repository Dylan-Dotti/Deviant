using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    public bool IsPaused { get; private set; }

    [SerializeField]
    private GameObject pauseMenuUI;

    private PlayerController pController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            pController = PlayerCharacter.Instance.Controller;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        pController.enabled = false;
        pauseMenuUI.SetActive(true);
    }

    private void OnDisable()
    {
        pauseMenuUI.SetActive(false);
        pController.enabled = true;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Manages the components of the start menu, 
 * including the title, buttons, and CPU
 */
public class StartMenu : MonoBehaviour
{
    public static StartMenu Instance { get; private set; }

    [SerializeField]
    private List<LerpWaveText> titleTextComponents;
    [SerializeField]
    private List<StartMenuButton> menuButtons;
    [SerializeField]
    private GameObject creditsDisplay;
    [SerializeField]
    private StartMenuCPU cpu;
    [SerializeField]
    private SceneTransitionPanel transitionPanel;
    [SerializeField]
    private LoadingBar loadBar;

    private TextGrid titleTextGrid;
    private TextGrid buttonsTextGrid;
    private TextLerpWave lerpWave;

    private KeyValuePair<TextLerpWave.WaveActivationSequence, Coroutine> activeButtonWave;

    private Coroutine titlePeriodicLerpRoutine;
    private Dictionary<TextLerpWave.WaveActivationSequence, Coroutine> titleActivations;

    private LerpWaveText mouseoverText;
    private AudioSource titleMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            titleActivations = new Dictionary<TextLerpWave.
                WaveActivationSequence, Coroutine>();

            //title grid
            List<List<LerpWaveText>> titleTextGridRows =
                new List<List<LerpWaveText>>();
            titleTextGridRows.Add(new List<LerpWaveText>(
                titleTextComponents));
            titleTextGrid = new TextGrid(titleTextGridRows);

            //buttons grid
            List<List<LerpWaveText>> buttonTextGridRows =
                new List<List<LerpWaveText>>();
            menuButtons.ForEach(b => buttonTextGridRows.Add(
                new List<LerpWaveText>(b.TextComponents)));
            buttonsTextGrid = new TextGrid(buttonTextGridRows);
            buttonsTextGrid.MouseEnterEvent += OnTextMouseEnter;
            buttonsTextGrid.MouseExitEvent += OnTextMouseExit;
            buttonsTextGrid.MouseClickEvent += OnTextMouseClick;

            lerpWave = GetComponent<TextLerpWave>();
            titleMusic = GetComponent<AudioSource>();

            Application.targetFrameRate = 60;
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

    public void OpenOptionsMenu()
    {

    }

    public void OpenCreditsMenu()
    {
        creditsDisplay.SetActive(true);
    }

    public void CloseCreditsMenu()
    {
        creditsDisplay.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnTextMouseEnter(LerpWaveText lwText)
    {
        mouseoverText = lwText;
        activeButtonWave = lerpWave.LerpForward(buttonsTextGrid, 
            lwText, 6, .05f, .3f);
    }

    private void OnTextMouseExit(LerpWaveText lwText)
    {
        mouseoverText = null;
        lerpWave.CancelLerp(activeButtonWave.Value);
        lerpWave.LerpReverse(activeButtonWave.Key);
    }

    private void OnTextMouseClick(LerpWaveText lwText)
    {

    }

    private IEnumerator InitCR()
    {
        transitionPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        titlePeriodicLerpRoutine = StartCoroutine(LerpTitleRandomPeriodic());
        titleMusic.Play();
        yield return transitionPanel.FadeTransparent();
        transitionPanel.gameObject.SetActive(false);
    }

    private IEnumerator LerpTitleRandomPeriodic()
    {
        FloatRange activateDelay = new FloatRange(1, 3);
        FloatRange deactivateDelay = new FloatRange(1, 2);
        while (true)
        {
            int randIndex = Random.Range(0, titleTextComponents.Count);
            LerpWaveText lerpBase = titleTextComponents[randIndex];
            KeyValuePair<TextLerpWave.WaveActivationSequence, Coroutine>
                waveSequencePair = lerpWave.LerpForward(titleTextGrid, lerpBase,
                3, .2f, 1f);
            lerpWave.ReverseSequenceAfter(waveSequencePair, 
                deactivateDelay.RandomRangeValue);
            yield return new WaitForSeconds(activateDelay.RandomRangeValue);
        }
    }

    private IEnumerator FillAllBlue(LerpWaveText baseText)
    {
        cpu.FillAllBlue();
        List<List<LerpWaveText>> textRows = new List<List<LerpWaveText>>();
        textRows.Add(titleTextComponents);
        menuButtons.ForEach(b => textRows.Add(b.TextComponents));
        TextGrid grid = new TextGrid(textRows);
        yield return lerpWave.LerpForward(grid, baseText, 10, 0, 0).Value;
    }

    private IEnumerator LoadGame()
    {
        yield return StartCoroutine(FillAllBlue(mouseoverText));
        transitionPanel.gameObject.SetActive(true);
        yield return transitionPanel.FadeOpaque();
        loadBar.gameObject.SetActive(true);
        AsyncOperation loadProgress = SceneManager.LoadSceneAsync(
            SceneManager.GetActiveScene().buildIndex + 1);
        loadProgress.allowSceneActivation = false;
        while (loadBar.LoadPercentage < 0.9f)
        {
            loadBar.LoadPercentage = loadProgress.progress;
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        loadProgress.allowSceneActivation = true;
        loadBar.LoadPercentage = 1;
    }
}

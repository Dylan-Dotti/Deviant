using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    //Move wave functionality to seperate class
    public class WaveActivationSequence
    {
        //public readonly LerpWaveText baseText;
        public readonly Stack<Dictionary<LerpWaveText,
            LerpWaveText.WaveForce>> activationStack;

        public WaveActivationSequence(LerpWaveText baseText,
            LerpWaveText.WaveForce baseForce)
        {
            activationStack = new Stack<Dictionary<
                LerpWaveText, LerpWaveText.WaveForce>>();
            Dictionary<LerpWaveText, LerpWaveText.WaveForce> baseDict =
                new Dictionary<LerpWaveText, LerpWaveText.WaveForce>();
            baseDict.Add(baseText, baseForce);
            activationStack.Push(baseDict);
        }

        public bool SequenceContainsText(LerpWaveText searchText)
        {
            foreach(Dictionary<LerpWaveText, LerpWaveText.WaveForce>
                dict in activationStack)
            {
                if (dict.ContainsKey(searchText))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public static StartMenu Instance { get; private set; }

    [SerializeField]
    private List<LerpWaveText> titleTextComponents;
    [SerializeField]
    private List<StartMenuButton> menuButtons;
    [SerializeField]
    private SceneTransitionPanel transitionPanel;
    [SerializeField]
    private LoadingBar loadBar;

    private TextGrid buttonsTextGrid;
    private WaveActivationSequence activeSequence;
    private Coroutine activeRoutine;

    private Coroutine titlePeriodicLerpRoutine;
    private Dictionary<WaveActivationSequence, Coroutine> titleActivations;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            List<List<LerpWaveText>> buttonTextGridRows =
                new List<List<LerpWaveText>>();
            menuButtons.ForEach(b => buttonTextGridRows.Add(
                new List<LerpWaveText>(b.TextComponents)));
            buttonsTextGrid = new TextGrid(buttonTextGridRows);
            buttonsTextGrid.MouseEnterEvent += OnTextMouseEnter;
            buttonsTextGrid.MouseExitEvent += OnTextMouseExit;
            buttonsTextGrid.MouseClickEvent += OnTextMouseClick;
            titleActivations = new Dictionary<WaveActivationSequence, Coroutine>();
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

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnTextMouseEnter(LerpWaveText lwText)
    {
        activeRoutine = StartCoroutine(LerpTextColorCascadeForward(lwText, 6));
    }

    private void OnTextMouseExit(LerpWaveText lwText)
    {
        StopCoroutine(activeRoutine);
        StartCoroutine(LerpTextColorCascadeReverse(activeSequence));
    }

    private void OnTextMouseClick(LerpWaveText lwText)
    {

    }

    private IEnumerator InitCR()
    {
        transitionPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        yield return transitionPanel.FadeForward();
        transitionPanel.gameObject.SetActive(false);
        titlePeriodicLerpRoutine = StartCoroutine(LerpTitleRandomPeriodic());
        /*foreach (LerpWaveText titleComponent in titleTextComponents)
        {
            titleComponent.ColorLerper.StartLerpPeriodic(
                new FloatRange(0.5f, 10f));
        }*/
    }

    private IEnumerator LerpTextColorCascadeForward(LerpWaveText baseText, int depth)
    {
        LerpWaveText.WaveForce baseTextForce = baseText.AddWaveForce(1);
        activeSequence = new WaveActivationSequence(baseText, baseTextForce);
        Stack<Dictionary<LerpWaveText, LerpWaveText.WaveForce>> activationStack =
            activeSequence.activationStack;

        for (int i = 0; i < depth; i++)
        {
            yield return new WaitForSeconds(0.05f);
            ICollection<LerpWaveText> previousActivated = activationStack.Peek().Keys;
            Dictionary<LerpWaveText, LerpWaveText.WaveForce> activated =
                new Dictionary<LerpWaveText, LerpWaveText.WaveForce>();
            foreach (LerpWaveText lwText in previousActivated)
            {
                ICollection<LerpWaveText> neighbors = buttonsTextGrid.GetTextNeighbors(lwText);
                foreach (LerpWaveText neighborText in neighbors)
                {
                    if (neighborText != null && !activeSequence
                        .SequenceContainsText(neighborText) &&
                        !activated.ContainsKey(neighborText))
                    {
                        LerpWaveText.WaveForce force;
                        if (neighborText.Row == baseText.Row)
                        {
                            force = neighborText.AddWaveForce(1f - (0.05f * 
                                buttonsTextGrid.GetHorizontalDistance(neighborText, baseText)));
                        }
                        else
                        {
                            force = neighborText.AddWaveForce(1f - (0.3f *
                                buttonsTextGrid.GetDiagonalDistance(neighborText, baseText)));
                        }
                        activated.Add(neighborText, force);
                    }
                }
            }
            if (activated.Count > 0)
            {
                activationStack.Push(activated);
            }
        }
    }

    private IEnumerator LerpTextColorCascadeReverse(WaveActivationSequence sequence)
    {
        Stack<Dictionary<LerpWaveText, LerpWaveText.WaveForce>> activationStack =
            sequence.activationStack;
        while (activationStack.Count > 0)
        {
            yield return new WaitForSeconds(0.25f);
            foreach (KeyValuePair<LerpWaveText, LerpWaveText.WaveForce>
                lwPair in activationStack.Pop())
            {
                lwPair.Key.RemoveWaveForce(lwPair.Value);
            }
        }
    }

    private IEnumerator LerpTitleRandomPeriodic()
    {
        while (true)
        {
            int randIndex = Random.Range(0, titleTextComponents.Count);
            LerpWaveText lerpBase = titleTextComponents[randIndex];
            WaveActivationSequence sequence = new WaveActivationSequence(
                lerpBase, new LerpWaveText.WaveForce(1));
            //todo
            yield return null;
        }
    }

    private IEnumerator ReverseSequenceAfter(WaveActivationSequence sequence,
        Coroutine sequenceRoutine, FloatRange delayRange)
    {
        yield return new WaitForSeconds(delayRange.RandomRangeValue);
        StopCoroutine(sequenceRoutine);
        StartCoroutine(LerpTextColorCascadeReverse(sequence));
    }

    private IEnumerator LoadGame()
    {
        transitionPanel.gameObject.SetActive(true);
        yield return transitionPanel.FadeReverse();
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

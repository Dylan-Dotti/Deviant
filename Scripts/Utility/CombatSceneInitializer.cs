using System.Collections;
using UnityEngine;

public class CombatSceneInitializer : MonoBehaviour
{
    [SerializeField]
    private SceneTransitionPanel transitionPanel;
    [SerializeField]
    private PlayerSpawnAnimation playerSpawner;
    [SerializeField]
    private GameObject audioManagerObject;
    [SerializeField]
    private GameObject playerUI;

    private void Start()
    {
        StartCoroutine(InitSequence());
    }

    private IEnumerator InitSequence()
    {
        transitionPanel.gameObject.SetActive(true);
        yield return transitionPanel.FadeTransparent();
        transitionPanel.gameObject.SetActive(false);
        yield return playerSpawner.PlayAnimation();
        playerUI.SetActive(true);
        audioManagerObject.SetActive(true);
        WaveGenerator.Instance.StartNextWave();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideShow : MonoBehaviour
{
    private List<Slide> slideSequence;

    private void Awake()
    {
        slideSequence = new List<Slide>();
        for (int i = 0; i < transform.childCount; i++)
        {
            slideSequence.Add(transform.GetChild(i).GetComponent<Slide>());
        }
    }

    private void Start()
    {
        StartCoroutine(SlideSequenceCR());
    }

    private IEnumerator SlideSequenceCR()
    {
        foreach (Slide slide in slideSequence)
        {
            slide.gameObject.SetActive(true);
            while (slide.NumComponentsRemaining > 0)
            {
                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null;
                }
                slide.ActivateNextComponent();
                yield return new WaitForSeconds(1);
            }
            yield return null;
            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }
            yield return null;
            if (slide != slideSequence[slideSequence.Count - 1])
            {
                slide.gameObject.SetActive(false);
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoSceneTransitioner : MonoBehaviour
{
    private static DemoSceneTransitioner Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            SceneManager.LoadScene(SceneManager.
                GetActiveScene().buildIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Equals))
        {
            SceneManager.LoadScene(SceneManager.
                GetActiveScene().buildIndex + 1);
        }
    }
}

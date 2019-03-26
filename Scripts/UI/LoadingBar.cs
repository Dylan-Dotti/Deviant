using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public float LoadPercentage
    {
        get => foreground.fillAmount;
        set => foreground.fillAmount = Mathf.Clamp01(value);
    }

    [SerializeField]
    private Image foreground;

    private void Awake()
    {
        LoadPercentage = 0;
    }
}

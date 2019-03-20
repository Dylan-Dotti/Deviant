using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrencyText : MonoBehaviour
{
    [SerializeField]
    private float baseUpdateInterval = 0.04f;

    private Text currencyText;
    private PlayerCharacter player;

    private int currentValue;

    private void Awake()
    {
        currencyText = GetComponent<Text>();
        player = PlayerCharacter.Instance;
    }

    private void OnEnable()
    {
        currentValue = player.NumSpareParts;
        currencyText.text = currentValue.ToString();
    }

    private void FixedUpdate()
    {
        currentValue += player.NumSpareParts.CompareTo(currentValue);
        currencyText.text = currentValue.ToString();
    }
}

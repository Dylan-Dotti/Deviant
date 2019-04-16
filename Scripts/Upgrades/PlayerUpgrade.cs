using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* Superclass of all player upgrades, represented in the upgrade menu 
 * as upgrade cards
 */
public abstract class PlayerUpgrade : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [System.Serializable]
    public class StatsDisplay
    {
        public Text CurrentStats { get => currentStats; }
        public Text NewStats { get => newStats; }
        public Image Arrow { get => arrow; }

        [SerializeField]
        private Text currentStats;
        [SerializeField]
        private Text newStats;
        [SerializeField]
        private Image arrow;

        public StatsDisplay(Text currentStats, Text newStats, Image arrow)
        {
            this.currentStats = currentStats;
            this.newStats = newStats;
            this.arrow = arrow;
        }
    }

    public virtual int Cost
    {
        get => cost;
        set => cost = Mathf.Max(0, value);
    }

    public virtual int MaxNumPurchases => int.MaxValue;

    public virtual string Description => descriptionText.text;

    public bool PlayerCanAfford
    {
        get => player.NumSpareParts >= Cost;
    }

    public virtual bool Purchasable
    {
        get => NumTimesPurchased < MaxNumPurchases;
    }

    public int NumTimesPurchased { get; private set; }

    [SerializeField]
    private int cost;

    protected PlayerCharacter player;

    private HashSet<StatsDisplay> statsDisplays;
    private Text descriptionText;
    private Text costText;
    private Text tierText;
    private Button purchaseButton;
    private AudioSource purchaseSound;
    private float purchaseCooldown = 0.5f;
    private float timeSinceLastPurchase;

    private Color32 originalCostTextColor;
    private Color32 unpurchasableTextColor =
        new Color32(255, 42, 42, 255);

    private bool isFading;

    protected virtual void Awake()
    {
        descriptionText = transform.Find("Description").GetComponent<Text>();
        costText = transform.Find("Purchase Bar").transform
            .Find("Cost").transform.Find("Cost Text").GetComponent<Text>();
        tierText = transform.Find("Tier Number").GetComponent<Text>();
        originalCostTextColor = costText.color;
        purchaseButton = GetComponentInChildren<Button>();
        purchaseButton.onClick.AddListener(AttemptApplyUpgrade);
        purchaseSound = GetComponent<AudioSource>();
        player = PlayerCharacter.Instance;
        statsDisplays = new HashSet<StatsDisplay>();
        UpdateTierText();
    }

    protected virtual void Update()
    {
        timeSinceLastPurchase += Time.deltaTime;
        descriptionText.text = Description;
        purchaseButton.interactable = PlayerCanAfford && Purchasable && Cost != 0;
        UpdateCostText();
        UpdateStatsDisplays();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetNewStatsActive(Purchasable);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetNewStatsActive(false);
    }

    public virtual void AttemptApplyUpgrade()
    {
        if (timeSinceLastPurchase >= purchaseCooldown)
        {
            ApplyUpgrade();
        }
    }

    public virtual void ApplyUpgrade()
    {
        player.NumSpareParts -= Cost;
        NumTimesPurchased++;
        UpdateTierText();
        if (NumTimesPurchased >= MaxNumPurchases)
        {
            FadeOut();
        }
        SetNewStatsActive(PlayerCanAfford && Purchasable);
        purchaseSound.PlayOneShot(purchaseSound.clip);
        timeSinceLastPurchase = 0;
    }

    /* Add stats display to be changed on mouseover */
    public bool AddStatsDisplay(StatsDisplay display)
    {
        if (statsDisplays.Add(display))
        {
            UpdateStatsDisplays();
            return true;
        }
        return false;
    }

    public bool RemoveStatsDisplay(StatsDisplay display)
    {
        if (statsDisplays.Remove(display))
        {
            display.Arrow.gameObject.SetActive(false);
            display.NewStats.gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    /* Set the new stats preview elements active on mouseover */
    public void SetNewStatsActive(bool active)
    {
        foreach (StatsDisplay statsDisplay in statsDisplays)
        {
            statsDisplay.Arrow.gameObject.SetActive(active);
            statsDisplay.NewStats.gameObject.SetActive(active);
        }
    }

    public void FadeIn()
    {
        Color32 startColor = GetComponent<Image>().color;
        StartCoroutine(FadeAlphaRecursive(startColor.a, 255f, 0.33f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeAlphaRecursive(255, 255f / 2.25f, 0.33f));
    }

    /* Update the player stats after a purchase */
    public abstract void UpdateStatsDisplays();

    private void UpdateCostText()
    {
        costText.text = Cost.ToString();
        if (!PlayerCanAfford && Purchasable)
        {
            costText.color = unpurchasableTextColor;
            costText.GetComponent<Outline>().enabled = false;
        }
        else
        {
            costText.color = originalCostTextColor;
            costText.GetComponent<Outline>().enabled = true;
        }
    }

    /* Updates the tier number after purchase */
    private void UpdateTierText()
    {
        if (tierText != null)
        {
            if (MaxNumPurchases == int.MaxValue)
            {
                tierText.text = "Tier " + NumTimesPurchased;
            }
            else
            {
                tierText.text = string.Format("Tier {0}/{1}",
                    NumTimesPurchased, MaxNumPurchases);
            }
        }
    }

    /* Fades out when the purchase limit is reached */
    private IEnumerator FadeAlphaRecursive(float startAlpha, float endAlpha, float duration)
    {
        while (isFading)
        {
            yield return null;
        }
        isFading = true;

        List<Image> imageComponents = new List<Image>(
            GetComponentsInChildren<Image>());
        List<Text> textComponents = new List<Text>(
            GetComponentsInChildren<Text>());

        float fadeStartTime = Time.time;
        while (Time.time - fadeStartTime < duration)
        {
            float lerpPercentage = (Time.time - fadeStartTime) / duration;
            byte newAlpha = (byte)Mathf.Lerp(startAlpha, endAlpha, lerpPercentage);
            foreach (Image image in imageComponents)
            {
                Color32 imageColor = image.color;
                image.color = new Color32(imageColor.r, imageColor.g, 
                    imageColor.b, newAlpha);
            }
            foreach (Text text in textComponents)
            {
                Color32 textColor = text.color;
                text.color = new Color32(textColor.r, textColor.g,
                    textColor.b, newAlpha);
            }

            originalCostTextColor = new Color32(originalCostTextColor.r,
                originalCostTextColor.g, originalCostTextColor.b, newAlpha);
            unpurchasableTextColor = new Color32(unpurchasableTextColor.r,
                unpurchasableTextColor.g, unpurchasableTextColor.b, newAlpha);

            yield return null;
        }
        isFading = false;
    }
}

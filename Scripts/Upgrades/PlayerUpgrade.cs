using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerUpgrade : MonoBehaviour
{
    public virtual int Cost
    {
        get { return cost; }
        set { cost = Mathf.Max(0, value); }
    }

    [SerializeField]
    protected Text descriptionText;
    [SerializeField]
    protected Button purchaseButton;
    [SerializeField]
    protected Text costText;
    [SerializeField]
    protected int cost;

    protected AudioSource purchaseSound;

    private float purchaseCooldown = 0.2f;
    private float timeSinceLastPurchase;

    protected virtual void Awake()
    {
        purchaseSound = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        timeSinceLastPurchase += Time.deltaTime;
        purchaseButton.interactable = Cost > 0;
        costText.text = Cost.ToString();
    }

    public void AttemptApplyUpgrade()
    {
        if (timeSinceLastPurchase >= purchaseCooldown)
        {
            ApplyUpgrade();
            timeSinceLastPurchase = 0;
        }
    }

    public virtual void ApplyUpgrade()
    {
        timeSinceLastPurchase = 0;
        purchaseSound.PlayOneShot(purchaseSound.clip);
    }
}

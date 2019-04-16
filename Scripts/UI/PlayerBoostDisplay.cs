using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoostDisplay : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> boostIcons;

    private List<Image> boostIconBackgrounds;
    private List<Image> boostIconForegrounds;
    private Boost playerBoost;

    private void Awake()
    {
        playerBoost = PlayerCharacter.Instance.Controller.Boost;
        boostIconBackgrounds = new List<Image>();
        boostIconForegrounds = new List<Image>();
        boostIcons.ForEach(i => boostIconBackgrounds.Add(
            i.transform.Find("Background").GetComponent<Image>()));
        boostIcons.ForEach(i => boostIconForegrounds.Add(
            i.transform.Find("Foreground").GetComponent<Image>()));
    }

    private void Update()
    {
        UpdateBackgrounds();
        UpdateForegrounds();
    }

    private void UpdateBackgrounds()
    {
        for (int i = 0; i < boostIcons.Count; i++)
        {
            Color bgColor = boostIconBackgrounds[i].color;

            if (i < playerBoost.MaxNumCharges)
            {
                boostIconBackgrounds[i].color = 
                    new Color(bgColor.r, bgColor.g, bgColor.b, 0.15686f);
            }
            else
            {
                boostIconBackgrounds[i].color = 
                    new Color(bgColor.r, bgColor.g, bgColor.b, 0.04706f);
            }
        }
    }

    private void UpdateForegrounds()
    {
        for (int i = 0; i < boostIcons.Count; i++)
        {
            if (i < playerBoost.CurrentNumCharges)
            {
                boostIconForegrounds[i].fillAmount = 1;
            }
            else if (i == playerBoost.CurrentNumCharges &&
                playerBoost.CurrentNumCharges != playerBoost.MaxNumCharges)
            {
                boostIconForegrounds[i].fillAmount =
                    playerBoost.CurrentChargeFillPercent;
            }
            else
            {
                boostIconForegrounds[i].fillAmount = 0;
            }
        }
    }
}

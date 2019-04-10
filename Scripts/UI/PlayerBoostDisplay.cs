using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoostDisplay : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> boostIcons;

    private List<Image> boostIconForegrounds;

    private Boost playerBoost;

    private void Awake()
    {
        boostIconForegrounds = new List<Image>();
        boostIcons.ForEach(i => boostIconForegrounds.Add(
            i.transform.Find("Foreground").GetComponent<Image>()));
        playerBoost = PlayerCharacter.Instance.Controller.Boost; 
    }

    private void Update()
    {
        for (int i = 0; i < boostIcons.Count; i++)
        {
            boostIcons[i].gameObject.SetActive(
                i < playerBoost.MaxNumCharges);
            if (i < playerBoost.CurrentNumCharges)
            {
                boostIconForegrounds[i].fillAmount = 1;
            }
            else if (i == playerBoost.CurrentNumCharges)
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

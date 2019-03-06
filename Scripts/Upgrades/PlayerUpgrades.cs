using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades
{
    public int HealthBonus { get; set; }
    public int BlasterDamageBonus { get; set; }
    public bool ExtraLife { get; set; }

    private int healthBonus;

    private PlayerCharacter player;

    public PlayerUpgrades()
    {
        player = PlayerCharacter.Instance;
    }
}

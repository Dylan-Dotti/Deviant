using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexSpawner : Character
{
    public override void Die()
    {
        Destroy(gameObject);
    }
}

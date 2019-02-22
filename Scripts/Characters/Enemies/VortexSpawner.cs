using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexSpawner : Enemy
{
    private VortexSpawnerDeathSequence deathSequence;

    protected override void Awake()
    {
        base.Awake();
        deathSequence = GetComponent<VortexSpawnerDeathSequence>();
    }

    public override void Die()
    {
        deathSequence.PlayAnimation();
    }
}

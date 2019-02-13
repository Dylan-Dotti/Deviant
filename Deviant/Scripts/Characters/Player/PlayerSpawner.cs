using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private PlayerCharacter player;
    
    [SerializeField]
    private GameObject spawnGraphics;
    [SerializeField]
    private ParticleSystem implosionParticles;
    [SerializeField]
    private GameObject playerGraphics;

    private void Awake()
    {
        player = GetComponentInChildren<PlayerCharacter>();
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnPlayer());
    }

    private IEnumerator SpawnPlayer()
    {
        spawnGraphics.SetActive(false);
        //player.enabled = false;
        spawnGraphics.SetActive(true);
        while (implosionParticles.particleCount < 1)
        {
            yield return null;
        }
        while (implosionParticles.particleCount > 0)
        {
            yield return null;
        }
        playerGraphics.SetActive(true);
        //player.enabled = true;
        enabled = false;
    }
}

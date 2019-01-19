using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private PlayerController playerController;
    
    [SerializeField]
    private GameObject spawnGraphics;
    [SerializeField]
    private GameObject playerGraphics;
    [SerializeField]
    private ParticleSystem implosionParticles;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        StartCoroutine(SpawnPlayer());
    }

    private IEnumerator SpawnPlayer()
    {
        playerController.enabled = false;
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
        playerController.enabled = true;
    }
}

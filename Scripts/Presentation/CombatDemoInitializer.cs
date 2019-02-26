﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDemoInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private GameObject UIObject;
    [SerializeField]
    private GameObject EnemySpawnerObject;

    private void Start()
    {
        playerObject.SetActive(false);
        UIObject.SetActive(false);
        EnemySpawnerObject.SetActive(false);
        StartCoroutine(InitCombatDemo());
    }

    private IEnumerator InitCombatDemo()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        playerObject.SetActive(true);
        UIObject.SetActive(true);
        yield return new WaitForSeconds(3);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        EnemySpawnerObject.SetActive(true);
        Destroy(gameObject);
    }
}

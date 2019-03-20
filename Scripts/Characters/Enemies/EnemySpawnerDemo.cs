using System.Collections;
using UnityEngine;

public class EnemySpawnerDemo : EnemySpawnerPeriodic
{
    [SerializeField]
    private GameObject musicObject;

    protected override void Start()
    {
        StartCoroutine(DemoSpawnSequence());
    }

    private IEnumerator DemoSpawnSequence()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            SpawnAndLaunchEnemy(spawns[0].EType);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        for (int i = 0; i < 2; i++)
        {
            SpawnAndLaunchEnemy(spawns[1].EType);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        for (int i = 0; i < 2; i++)
        {
            SpawnAndLaunchEnemy(spawns[2].EType);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(3);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        SpawnAndLaunchEnemy(spawns[3].EType);
        yield return new WaitForSeconds(3);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        SpawnAndLaunchEnemy(spawns[4].EType);
        yield return new WaitForSeconds(3);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        musicObject.gameObject.SetActive(true);
        SpawnPeriodically();
    }
}

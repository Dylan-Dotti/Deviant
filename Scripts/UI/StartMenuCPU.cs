using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuCPU : MonoBehaviour
{
    [SerializeField]
    private StartMenuCPUComponent playerBody;
    [SerializeField]
    private List<StartMenuCPUComponent> playerCircuitNodes;
    [SerializeField]
    private StartMenuCPUComponent cpuCube;

    private List<StartMenuCPUComponent> cpuCircuits;
    //private Dictionary<List<Renderer>, Coroutine> circuitLerpRoutines;

    private readonly string fresnelColorProperty = "Color_AF7F0660";
    private readonly string fresnelStrengthProperty = "Vector1_7BA6298F";

    private void Awake()
    {
        cpuCircuits = new List<StartMenuCPUComponent>();
        List<Transform> circuitGroups = new List<Transform>
        {
            transform.Find("Top Circuits"),
            transform.Find("Right Circuits"),
            transform.Find("Bottom Circuits"),
            transform.Find("Left Circuits")
        };
        foreach (Transform cGroup in circuitGroups)
        {
            for (int i = 0; i < cGroup.childCount; i++)
            {
                cpuCircuits.Add(cGroup.GetChild(i).
                    GetComponent<StartMenuCPUComponent>());
            }
        }
    }

    private void Start()
    {
        foreach (StartMenuCPUComponent circuit in cpuCircuits)
        {
            circuit.LerpColorPeriodic(new FloatRange(0.5f, 1),
                new FloatRange(0.5f, 15), new FloatRange(1, 3),
                new FloatRange(0.5f, 4));
            circuit.LerpFresnelStrengthPeriodic(-3, new FloatRange(1, 3),
                new FloatRange(0.5f, 7));
        }
        StartCoroutine(LerpCPUBodyPeriodicCR());
        StartCoroutine(FixMemoryLeakPerodicCR());
    }

    public void FillAllBlue()
    {
        StopAllCoroutines();
        StartCoroutine(LerpAllBlueCR());
    }

    private IEnumerator LerpAllBlueCR()
    {
        playerBody.CancelLerping();
        playerBody.LerpColorBlue(4);
        playerCircuitNodes.ForEach(n => { n.CancelLerping(); n.LerpColorBlue(4); });
        cpuCube.CancelLerping();
        cpuCube.LerpColorBlue(4);
        foreach (StartMenuCPUComponent circuit in cpuCircuits)
        {
            circuit.CancelLerping();
            circuit.LerpColorBlue(3);
            yield return null;
        }
    }

    private IEnumerator LerpCPUBodyPeriodicCR()
    {
        playerBody.LerpFresnelStrengthPeriodic(1.5f, new FloatRange(0.5f, 2f),
            new FloatRange(1f, 2f));
        cpuCube.LerpFresnelStrengthPeriodic(0.15f, new FloatRange(0.5f, 2f),
            new FloatRange(1f, 2f));
        yield return new WaitForSeconds(5);
        FloatRange r2bInterval = new FloatRange(3.5f, 8);
        FloatRange b2rInterval = new FloatRange(1, 3);
        while (true)
        {
            float duration = Random.Range(1.5f, 2f);
            playerBody.LerpColorBlue(duration);
            playerCircuitNodes.ForEach(n => n.LerpColorBlue(duration));
            yield return new WaitForSeconds(0.1f);
            cpuCube.LerpColorBlue(duration);
            yield return new WaitForSeconds(b2rInterval.RandomRangeValue);
            cpuCube.LerpColorRed(duration);
            yield return new WaitForSeconds(0.1f);
            playerBody.LerpColorRed(duration);
            playerCircuitNodes.ForEach(n => n.LerpColorRed(duration));
            yield return new WaitForSeconds(r2bInterval.RandomRangeValue);
        }
    }

    /*private IEnumerator LerpCPUBodyBlue()
    {
        playerBody.LerpForward();
        playerCircuitNodeLerpers.ForEach(l => l.LerpForward());
        yield return new WaitForSeconds(.25f);
        cubeLerper.LerpForward();
    }*/

    private IEnumerator FixMemoryLeakPerodicCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            Resources.UnloadUnusedAssets();
        }
    }
}

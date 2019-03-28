using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuCPU : MonoBehaviour
{
    [SerializeField]
    private FresnelMaterialLerp playerBodyLerper;
    [SerializeField]
    private List<FresnelMaterialLerp> playerCircuitNodeLerpers;
    [SerializeField]
    private FresnelMaterialLerp cubeLerper;

    public Coroutine LerpBlue()
    {
        return StartCoroutine(LerpPlayerBodyBlue());
    }

    private IEnumerator LerpPlayerBodyBlue()
    {
        playerBodyLerper.LerpForward();
        playerCircuitNodeLerpers.ForEach(l => l.LerpForward());
        yield return new WaitForSeconds(.25f);
        cubeLerper.LerpForward();
        yield return null;
    }
}

using UnityEngine;

public class VortexLauncher : Weapon
{
    [SerializeField]
    private Vortex vortexPrefab;

    public override void FireWeapon()
    {
        base.FireWeapon();
        Vector3 launchDirection = new Vector3(Random.Range(-1f, 1f),
            transform.position.y, Random.Range(-1f, 1f)).normalized;
        Vortex vortex = Instantiate(vortexPrefab);
        vortex.transform.position = transform.position;
        Vector3 targetPos = launchDirection * Random.Range(6f, 20f);
        vortex.MoveToPosition(targetPos);
    }
}

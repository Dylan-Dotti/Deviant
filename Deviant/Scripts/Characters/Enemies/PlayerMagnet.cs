using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    public enum MagnetMode { Attract, Repel }

    [SerializeField]
    private MagnetMode magnetMode;
    [SerializeField]
    private VelocityModifier magnetVelocityModifier;

    [SerializeField]
    private RangeFloat falloffRadiusRange;

    private PlayerCharacter player;

    private void Awake()
    {
        player = PlayerCharacter.Instance;
    }

    private void OnEnable()
    {
        player.Controller.AddVelocityModifier(magnetVelocityModifier);
    }

    private void OnDisable()
    {
        player.Controller.RemoveVelocityModifier(magnetVelocityModifier);
    }

    private void FixedUpdate()
    {
        float playerDist = Vector3.Distance(transform.position, 
            player.transform.position);
        float forceMagnitude;
        if (playerDist < falloffRadiusRange.min)
        {
            forceMagnitude = magnetVelocityModifier.MaxMagnitude;
        }
        else if (playerDist > falloffRadiusRange.max)
        {
            forceMagnitude = 0f;
        }
        else
        {
            float playerDistFromMin = playerDist - falloffRadiusRange.min;
            float percentageMaxDist = playerDistFromMin / 
                (falloffRadiusRange.max - falloffRadiusRange.min);
            forceMagnitude = Mathf.Lerp(magnetVelocityModifier.MaxMagnitude,
                0f, percentageMaxDist);
        }
        magnetVelocityModifier.Velocity = GetForceDirection() * forceMagnitude;
    }

    private Vector3 GetForceDirection()
    {
        Vector3 playerPosition = player.transform.position;
        switch (magnetMode)
        {
            case MagnetMode.Attract:
                return new Vector3(transform.position.x - playerPosition.x, playerPosition.y,
                    transform.position.z - playerPosition.z).normalized;
            case MagnetMode.Repel:
                return new Vector3(playerPosition.x - transform.position.x, playerPosition.y,
                    transform.position.z - playerPosition.z).normalized;
            default:
                return Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, falloffRadiusRange.min);
        Gizmos.DrawWireSphere(transform.position, falloffRadiusRange.max);
    }
}

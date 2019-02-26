using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    public enum MagnetMode { Attract, Repel }

    public float ForceMagnitude
    {
        get
        {
            return magnetVelocityModifier.Velocity.magnitude;
        }
        set
        {
            magnetVelocityModifier.Velocity = Vector3.ClampMagnitude(
                magnetVelocityModifier.Velocity, value);
        }
    }
    public FloatRange FalloffRadiusRange
    {
        get { return falloffRadiusRange; }
        set { falloffRadiusRange = value; }
    }

    [SerializeField]
    private MagnetMode magnetMode;
    [SerializeField]
    private VelocityModifier magnetVelocityModifier;

    [SerializeField]
    private FloatRange falloffRadiusRange;

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
        if (playerDist < falloffRadiusRange.Min)
        {
            forceMagnitude = magnetVelocityModifier.MaxMagnitude;
        }
        else if (playerDist > falloffRadiusRange.Max)
        {
            forceMagnitude = 0f;
        }
        else
        {
            float playerDistFromMin = playerDist - falloffRadiusRange.Min;
            float percentageMaxDist = playerDistFromMin / 
                (falloffRadiusRange.Max - falloffRadiusRange.Min);
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
                    playerPosition.z - transform.position.z).normalized;
            default:
                return Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, falloffRadiusRange.Min);
        Gizmos.DrawWireSphere(transform.position, falloffRadiusRange.Max);
    }
}

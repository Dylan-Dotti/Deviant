using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : MonoBehaviour
{
    [SerializeField]
    private float pullFalloffRadius = 1f;
    [SerializeField]
    private float pullFalloffGradientSpacing = 1f;

    [SerializeField]
    private VelocityModifier vortexVelocityModifier;

    private PlayerCharacter player;

    private void Awake()
    {
        Debug.Log("Vortex Awake");
        player = PlayerCharacter.Instance;
    }

    private void OnEnable()
    {
        player.Controller.AddVelocityModifier(vortexVelocityModifier);
    }

    private void OnDisable()
    {
        player.Controller.RemoveVelocityModifier(vortexVelocityModifier);
    }

    private void FixedUpdate()
    {
        Vector3 pullDirection = new Vector3(transform.position.x -
            player.transform.position.x, 0, transform.position.z -
            player.transform.position.z).normalized;
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);
        float pullMagnitude = playerDistance > pullFalloffRadius ? vortexVelocityModifier.MaxMagnitude / 
            ((playerDistance - pullFalloffRadius) / pullFalloffGradientSpacing + 1) : 
            vortexVelocityModifier.MaxMagnitude;
        vortexVelocityModifier.Velocity = pullDirection * pullMagnitude;
        //Debug.Log(pullMagnitude);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool KeyboardInputEnabled { get; set; } = true;
    public bool MovementEnabled { get; set; } = true;
    public bool MouseRotateEnabled { get; set; } = true;
    public bool WeaponEnabled { get; set; } = true;
    public bool BoostEnabled { get; set; } = true;

    public float Acceleration { get { return acceleration; } set { acceleration = value; } }
    public float Drag { get { return drag; } set { drag = value; } }

    public Vector3 MoveVelocity { get { return playerVelocityModifier.Velocity; } }
    public Vector3 TotalVelocity { get { return rBody.velocity; } }
    public Vector3 TotalLocalVelocity
    {
        get
        {
            float xDot = Vector3.Dot(TotalVelocity, rotator.transform.right);
            float zDot = Vector3.Dot(TotalVelocity, rotator.transform.forward);
            return new Vector3(xDot, 0f, zDot);
        }
    }
    public float MaxVelocityMagnitude
    {
        get { return playerVelocityModifier.MaxMagnitude; }
        set { playerVelocityModifier.MaxMagnitude = value; }
    }

    [SerializeField]
    private Weapon playerWeapon;

    [SerializeField]
    private float acceleration = 8f;
    [SerializeField]
    private float drag = 4f;

    [SerializeField]
    private VelocityModifier playerVelocityModifier;
    private HashSet<VelocityModifier> vModifiers;

    [SerializeField]
    private LerpRotationToTarget rotator;

    private Boost boost;
    private Rigidbody rBody;

    private void Awake()
    {
        boost = GetComponent<Boost>();
        rBody = GetComponent<Rigidbody>();
        vModifiers = new HashSet<VelocityModifier>();
        AddVelocityModifier(playerVelocityModifier);
    }

    private void Update()
    {
        //print framerate
        //Debug.Log(1 / Time.deltaTime);

        if (KeyboardInputEnabled && Input.GetKeyUp(KeyCode.Escape))
        {
            PauseMenu.Instance.PauseGame();
        }
    }

    private void FixedUpdate()
    {
        if (KeyboardInputEnabled)
        {
            if (MovementEnabled)
            {
                MovePlayer();
            }
            if (MouseRotateEnabled)
            {
                RotatePlayer();
            }
            if (WeaponEnabled)
            {
                FireWeapon();
            }
        }
    }

    public void ResetMovement()
    {
        boost.CancelBoost();
        playerVelocityModifier.Velocity = Vector3.zero;
    }

    public void CancelBoost()
    {
        boost.CancelBoost();
    }

    public bool AddVelocityModifier(VelocityModifier modifier)
    {
        return vModifiers.Add(modifier);
    }

    public bool RemoveVelocityModifier(VelocityModifier modifier)
    {
        return vModifiers.Remove(modifier);
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (BoostEnabled && Input.GetKeyDown(KeyCode.Space))
        {
            boost.ActivateBoost(moveDirection);
        }

        Vector3 newVelocity = playerVelocityModifier.Velocity + moveDirection * Acceleration * Time.fixedDeltaTime;
        if (newVelocity.magnitude > 0)
        {
            float dragMagnitude = Mathf.Clamp(Mathf.Pow(playerVelocityModifier.Velocity.magnitude / 
                playerVelocityModifier.MaxMagnitude, 2) * Drag, 0.15f * Drag, Drag);
            Vector3 dragVelocity = -newVelocity.normalized * dragMagnitude * Time.fixedDeltaTime;
            float newX = newVelocity.x > 0 ?
                Mathf.Max(newVelocity.x + dragVelocity.x, 0) :
                Mathf.Min(newVelocity.x + dragVelocity.x, 0);
            float newZ = newVelocity.z > 0 ?
                Mathf.Max(newVelocity.z + dragVelocity.z, 0) :
                Mathf.Min(newVelocity.z + dragVelocity.z, 0);
            newVelocity = new Vector3(newX, 0, newZ);
        }
        playerVelocityModifier.Velocity = newVelocity;

        rBody.velocity = Vector3.zero;
        foreach (VelocityModifier modifier in vModifiers)
        {
            rBody.velocity += modifier.Velocity;
        }
        //Debug.Log(transform.InverseTransformDirection(MoveVelocity));
        //Debug.Log(TotalLocalVelocity);
    }

    private void RotatePlayer()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rotator.TargetPosition = mousePos;
    }

    private void RotateWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerWeapon.TurnToFace(mousePos);
    }

    private void FireWeapon()
    {
        if (Input.GetMouseButton(0))
        {
            playerWeapon.AttemptFireWeapon();
        }
    }
}

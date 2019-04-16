using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* Manages all things related to player controls, including 
 * firing weapons and moving the player character in the world. 
 */
public class PlayerController : MonoBehaviour
{
    public IEnumerable<Weapon> Weapons => playerWeapons;
    public Weapon EquippedWeapon { get; private set; }

    public UnityAction<Weapon> WeaponEquippedEvent;

    // switches for enabling/disabling different aspects of control
    public bool PlayerInputEnabled { get; set; } = true;
    public bool MovementEnabled { get; set; } = true;
    public bool MouseRotateEnabled { get; set; } = true;
    public bool WeaponEnabled { get; set; } = true;
    public bool BoostEnabled { get; set; } = true;

    public float Acceleration
    {
        get => acceleration;
        set => acceleration = value;
    }
    public float Drag
    {
        get => drag;
        set => drag = value;
    }

    public Vector3 MoveVelocity { get => playerVelocityModifier.Velocity; }
    public Vector3 TotalVelocity { get => rBody.velocity; }
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
        get => playerVelocityModifier.MaxMagnitude;
        set => playerVelocityModifier.MaxMagnitude = value;
    }

    public Boost Boost { get; private set; }
    public LerpRotationToTarget Rotator { get => rotator; }

    [SerializeField]
    private List<Weapon> playerWeapons;

    [SerializeField]
    private float acceleration = 8f;
    [SerializeField]
    private float drag = 4f;

    [SerializeField]
    private VelocityModifier playerVelocityModifier;
    private HashSet<VelocityModifier> vModifiers;

    [SerializeField]
    private LerpRotationToTarget rotator;

    private Rigidbody rBody;

    private void Awake()
    {
        Boost = GetComponent<Boost>();
        rBody = GetComponent<Rigidbody>();
        vModifiers = new HashSet<VelocityModifier>();
        AddVelocityModifier(playerVelocityModifier);
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        foreach (Weapon weapon in playerWeapons)
        {
            if (weapon.gameObject.activeSelf)
            {
                EquipWeapon(weapon);
                break;
            }
        }
    }

    // check for player input each frame
    private void Update()
    {
        if (PlayerInputEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu.Instance.PauseGame();
            }
            else if ((Input.GetKey(KeyCode.LeftShift) || 
                Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.R))
            {
                UpgradeMenu.Instance.enabled = true;
            }

            if (WeaponEnabled)
            {
                FireWeapon();
            }

            SwitchWeapons();
        }
    }

    // Movements involving physics should be placed in FixedUpdate 
    private void FixedUpdate()
    {
        ApplyDrag();
        if (PlayerInputEnabled)
        {
            if (MovementEnabled)
            {
                MovePlayer();
            }
            if (MouseRotateEnabled)
            {
                RotatePlayer();
            }
        }
    }

    public void ResetMovement()
    {
        Boost.CancelBoost();
        playerVelocityModifier.Velocity = Vector3.zero;
    }

    public void CancelBoost()
    {
        Boost.CancelBoost();
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

        //boost player
        if (BoostEnabled && moveDirection.magnitude > 0 &&
            Input.GetKeyDown(KeyCode.Space))
        {
            Boost.AttemptBoost(moveDirection);
        }

        Vector3 newVelocity = playerVelocityModifier.Velocity + 
            moveDirection * Acceleration * Time.fixedDeltaTime;
        playerVelocityModifier.Velocity = newVelocity;

        rBody.velocity = Vector3.zero;
        foreach (VelocityModifier modifier in vModifiers)
        {
            rBody.velocity += modifier.Velocity;
        }
    }

    /* Due to using velocity modifiers instead of the physics functions 
     * for movement, drag needs to be simulated manually 
     */
    private void ApplyDrag()
    {
        Vector3 playerVelocity = MoveVelocity;
        if (playerVelocity.magnitude > 0)
        {
            float dragMagnitude = Mathf.Clamp(Mathf.Pow(
                playerVelocityModifier.Velocity.magnitude / 
                playerVelocityModifier.MaxMagnitude, 2) * Drag, 0.15f * Drag, Drag);
            Vector3 dragVelocity = -playerVelocity.normalized * 
                dragMagnitude * Time.fixedDeltaTime;

            float newX = playerVelocity.x > 0 ?
                Mathf.Max(playerVelocity.x + dragVelocity.x, 0) :
                Mathf.Min(playerVelocity.x + dragVelocity.x, 0);
            float newZ = playerVelocity.z > 0 ?
                Mathf.Max(playerVelocity.z + dragVelocity.z, 0) :
                Mathf.Min(playerVelocity.z + dragVelocity.z, 0);
            playerVelocityModifier.Velocity = new Vector3(newX, 0, newZ);
        }

        rBody.velocity = Vector3.zero;
        foreach (VelocityModifier modifier in vModifiers)
        {
            rBody.velocity += modifier.Velocity;
        }
    }

    private void RotatePlayer()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rotator.TargetPosition = mousePos;
    }

    private void RotateWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        EquippedWeapon.TurnToFace(mousePos);
    }

    private void FireWeapon()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EquippedWeapon.CancelFireWeapon();
        }
        else if (Input.GetMouseButton(0))
        {
            EquippedWeapon.AttemptFireWeapon();
        }
    }

    private void SwitchWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(playerWeapons[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(playerWeapons[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(playerWeapons[2]);
        }
    }

    private void EquipWeapon(Weapon newWeapon)
    {
        if (EquippedWeapon != newWeapon)
        {
            EquippedWeapon?.gameObject.SetActive(false);
            EquippedWeapon = newWeapon;
            EquippedWeapon.gameObject.SetActive(true);
            WeaponEquippedEvent?.Invoke(newWeapon);
        }
    }
}

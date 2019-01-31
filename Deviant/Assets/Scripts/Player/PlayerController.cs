using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool MovementEnabled { get; set; }
    public bool MouseRotateEnabled { get; set; }
    public bool WeaponEnabled { get; set; }

    public float MaxVelocity { get; set; }
    public float Acceleration { get; set; }

    [SerializeField]
    private Weapon playerWeapon;

    private Rigidbody rBody;

    private void Awake()
    {
        MovementEnabled = true;
        MouseRotateEnabled = true;
        WeaponEnabled = true;
        MaxVelocity = 4;
        Acceleration = 10;
        rBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //print framerate
        //Debug.Log(1 / Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //PauseMenu.Instance
        }
        else
        {
            //RotateWeapon();
            if (WeaponEnabled)
            {
                FireWeapon();
            }
        }
    }

    private void FixedUpdate()
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

    private void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        rBody.AddForce(moveDirection * Acceleration, ForceMode.Force);
        if (rBody.velocity.magnitude > MaxVelocity)
        {
            rBody.velocity = rBody.velocity.normalized * MaxVelocity;
        }
    }

    private void RotatePlayer()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        Vector3 newRotation = (angle - 90f) * Vector3.forward;
        //rBody.MoveRotation(Quaternion.Euler(newRotation));
        transform.localRotation = Quaternion.Euler(newRotation);
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

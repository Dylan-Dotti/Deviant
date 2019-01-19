using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MaxVelocity { get; set; }
    public float Acceleration { get; set; }

    [SerializeField]
    private Weapon playerWeapon;
    [SerializeField]
    private GameObject playerGraphics;

    private Rigidbody rBody;

    private void Start()
    {
        MaxVelocity = 4;
        Acceleration = 10;
        rBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //print framerate
        Debug.Log(1 / Time.deltaTime);

        RotatePlayer();
        //RotateWeapon();
        FireWeapon();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        playerGraphics.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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

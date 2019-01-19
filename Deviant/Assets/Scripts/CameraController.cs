using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool FollowPlayer { get; set; }

    [SerializeField]
    private Transform playerTransform;

	void Update ()
    {
        transform.position = new Vector3(playerTransform.position.x,
            playerTransform.position.y, transform.position.z);
	}
}

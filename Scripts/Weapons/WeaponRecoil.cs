using System.Collections;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [SerializeField]
    private Vector3 forwardDirection = Vector3.forward;
    [SerializeField]
    private float recoilDistance = 0.1f;
    [SerializeField]
    private float recoilRecoverMoveDelta = 0.01f;
    [SerializeField]
    private float recoilCooldown;

    private Vector3 recoilResetPos;
    private float timeSinceLastRecoil;

    private void Start()
    {
        recoilResetPos = transform.localPosition;
        if (recoilCooldown == 0)
        {
            enabled = false;
        }
        StartCoroutine(RecoilRecover());
    }

    private void Update()
    {
        timeSinceLastRecoil += Time.deltaTime;
    }

    public void Recoil()
    {
        transform.localPosition = transform.localPosition +
            recoilDistance * -forwardDirection;
    }

    public void AttemptRecoil()
    {
        if (timeSinceLastRecoil >= recoilCooldown)
        {
            Recoil();
            timeSinceLastRecoil = 0;
        }
    }

    private IEnumerator RecoilRecover()
    {
        while (true)
        {
            Vector3 newLocalPos = transform.localPosition +
                forwardDirection * recoilRecoverMoveDelta;
            if (Vector3.Dot(newLocalPos - recoilResetPos, forwardDirection) >= 0)
            {
                newLocalPos = recoilResetPos;
            }
            transform.localPosition = newLocalPos;
            yield return null;
        }
    }
}

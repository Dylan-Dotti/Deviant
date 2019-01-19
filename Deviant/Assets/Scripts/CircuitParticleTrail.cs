using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitParticleTrail : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem trail;
    [SerializeField]
    private ParticleSystem node;
    [SerializeField]
    private float duration = 1.0f;
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private List<float> curveTimes;
    [SerializeField]
    private List<float> curveAngles;

    private float elapsedTime = 0.0f;

    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        yield return new WaitForSeconds(1f);
        //travel forward and check for curve
        while (elapsedTime <= duration)
        {
            if (curveTimes.Count > 0 && elapsedTime >= curveTimes[0])
            {
                transform.eulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + curveAngles[0]);
                curveTimes.RemoveAt(0);
                curveAngles.RemoveAt(0);
            }
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //explode
        node.Play();
        while (node.particleCount < 1)
        {
            yield return null;
        }
        while (node.particleCount > 0)
        {
            yield return null;
        }
        Destroy(gameObject, 1f);
    }
}

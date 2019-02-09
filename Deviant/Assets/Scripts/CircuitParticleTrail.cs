using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitParticleTrail : MonoBehaviour
{
    [System.Serializable]
    public class Curve
    {
        public float time;
        public float angle;

        public Curve(float time, float angle)
        {
            this.time = time;
            this.angle = angle;
        }
    }

    [SerializeField]
    private ParticleSystem trail;
    [SerializeField]
    private ParticleSystem node;

    [SerializeField]
    private float duration = 1.0f;
    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float startAngle = 0f;
    [SerializeField]
    private List<Curve> curves;

    private void Start()
    {
        curves.Insert(0, new Curve(0f, startAngle));
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        yield return new WaitForSeconds(1f);
        List<Curve> curvesCopy = new List<Curve>(curves);
        float elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            if (curvesCopy.Count > 0 && elapsedTime >= curvesCopy[0].time)
            {
                transform.eulerAngles = new Vector3(0f, transform.localEulerAngles.y + curvesCopy[0].angle, 0f);
                curvesCopy.RemoveAt(0);
            }
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
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
        gameObject.SetActive(false);
    }
}

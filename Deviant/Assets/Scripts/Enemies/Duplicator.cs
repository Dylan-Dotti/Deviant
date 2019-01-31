using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplicator : Character
{
    [SerializeField]
    private Duplicator duplicatorPrefab;

    private Rigidbody rBody;
    private static int numDuplicators = 0;

    protected override void Awake()
    {
        base.Awake();
        numDuplicators++;
        rBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(DuplicatePeriodically());
    }

    private void OnDestroy()
    {
        numDuplicators--;
    }

    public override void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DuplicatePeriodically()
    {
        List<Collider> colliders = new List<Collider>();
        colliders.AddRange(GetComponentsInChildren<Collider>());
        while (true)
        {
            if (numDuplicators < 200)
            {
                yield return new WaitForSeconds(Random.Range(3f, 5f));
                Duplicator clone = Instantiate(duplicatorPrefab, transform.position, transform.rotation);
                Vector3 separationVelocity = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f))
                    .normalized * Random.Range(1f, 2f);
                rBody.velocity = separationVelocity;
                clone.rBody.velocity = -separationVelocity;

                foreach(Collider c in colliders) c.isTrigger = true;
                yield return new WaitForSeconds(0.25f);
                foreach (Collider c in colliders) c.isTrigger = false;
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator DeathSequence()
    {
        List<GameObject> childObjects = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects.Add(transform.GetChild(i).gameObject);
        }

        GetComponent<Collider>().enabled = false;
        foreach (GameObject childGO in childObjects)
        {
            //alter components
            Collider childCollider = childGO.GetComponent<Collider>();
            if (childCollider != null)
            {
                childCollider.enabled = false;
            }
            Rigidbody childRB = childGO.AddComponent<Rigidbody>();
            childRB.useGravity = false;
            childRB.drag = 1f;

            //add forces
            Vector3 forceDirection = new Vector3(childGO.transform.position.x - transform.position.x,
                -0.2f, childGO.transform.position.z - transform.position.z).normalized;
            childRB.AddForce(forceDirection * Random.Range(0.1f, 1f), ForceMode.Impulse);
            float torqueDirectionScalar = Random.value > 0.5f ? 1 : -1;
            childRB.AddTorque(Random.Range(0.25f, 0.5f) * torqueDirectionScalar * Vector3.up, ForceMode.Impulse);
        }

        //shrink
        yield return new WaitForSeconds(1.5f);
        float shrinkDuration = 2.0f;
        float shrinkStartTime = Time.time;
        while (Time.time - shrinkStartTime < shrinkDuration)
        {
            float lerpPercentage = Mathf.Min(Time.time - shrinkStartTime, shrinkDuration) / shrinkDuration;
            foreach (GameObject childGO in childObjects)
            {
                childGO.transform.localScale = Vector3.Lerp(childGO.transform.localScale, Vector3.zero, lerpPercentage);
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}

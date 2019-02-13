using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Duplicator : Enemy
{
    public static CharacterDelegate DuplicatorDeathEvent;

    private static int numDuplicators = 0;

    [SerializeField]
    private Duplicator duplicatorPrefab;

    private Rigidbody rBody;

    protected override void Awake()
    {
        base.Awake();
        numDuplicators++;
        rBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //StartCoroutine(DuplicatePeriodically());
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

    public Duplicator Duplicate()
    {
        Duplicator clone = Instantiate(duplicatorPrefab, transform.position, transform.rotation);
        StartCoroutine(SeparateFromClone(clone));
        return clone;
    }

    private IEnumerator SeparateFromClone(Duplicator clone)
    {
        Vector3 separationVelocity = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f))
            .normalized * Random.Range(1f, 2f);
        rBody.velocity = separationVelocity;
        clone.rBody.velocity = -separationVelocity;

        List<Collider> colliders = new List<Collider>();
        colliders.AddRange(GetComponentsInChildren<Collider>());
        foreach (Collider c in colliders) c.isTrigger = true;
        yield return new WaitForSeconds(0.25f);
        foreach (Collider c in colliders) c.isTrigger = false;
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
        DuplicatorDeathEvent?.Invoke(this);
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
        yield return new WaitForSeconds(1f);
        List<Transform> childTransforms = childObjects.Select(o => o.transform).ToList();
        float shrinkDuration = 0.75f;
        LerpScaleOverDuration(childTransforms, 0f, shrinkDuration);
        yield return new WaitForSeconds(shrinkDuration + 0.05f);
        Destroy(gameObject);
    }
}

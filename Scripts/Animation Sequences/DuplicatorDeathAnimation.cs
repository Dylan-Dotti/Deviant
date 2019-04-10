using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Duplicator))]
public class DuplicatorDeathAnimation : AnimationSequence
{
    [SerializeField]
    private List<Transform> objectsToLaunch;
    [SerializeField]
    private HealthBar healthBar;

    private Duplicator duplicator;

    private void Awake()
    {
        duplicator = GetComponent<Duplicator>();
    }

    protected override IEnumerator AnimationSequenceCR()
    {
        IsPlaying = true;
        healthBar.gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        foreach (Transform objTransform in objectsToLaunch)
        {
            //alter components
            Collider childCollider = objTransform.GetComponent<Collider>();
            if (childCollider != null)
            {
                childCollider.enabled = false;
            }
            Rigidbody childRB = objTransform.gameObject.AddComponent<Rigidbody>();
            childRB.useGravity = false;
            childRB.drag = 1f;

            //add forces
            Vector3 forceDirection = new Vector3(objTransform.transform.position.x -
                transform.position.x, -0.2f, objTransform.transform.position.z -
                transform.position.z).normalized;
            childRB.AddForce(forceDirection * Random.Range(
                0.1f, 1f), ForceMode.Impulse);
            float torqueDirectionScalar = Random.value > 0.5f ? 1 : -1;
            childRB.AddTorque(Random.Range(0.25f, 0.5f) *
                torqueDirectionScalar * Vector3.up, ForceMode.Impulse);
        }

        //shrink
        yield return new WaitForSeconds(1f);
        yield return duplicator.LerpScaleOverDuration(objectsToLaunch, 0f, 0.75f);
        Destroy(gameObject);
    }
}

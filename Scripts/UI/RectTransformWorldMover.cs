using System.Collections;
using UnityEngine;

public abstract class RectTransformWorldMover : MonoBehaviour
{
    [SerializeField]
    private FloatRange moveSpeedRange = new FloatRange(1, 2);
    [SerializeField]
    private float duration = 1f;

    private RectTransform rTransform;

    protected virtual void Awake()
    {
        rTransform = GetComponent<RectTransform>();
    }

    public virtual void SpawnAtPos(Vector3 spawnPos)
    {
        transform.position = spawnPos;
        StartCoroutine(TranslateAndFadeOut());
    }

    public abstract void SetAlpha(float percentage);

    public abstract void RemoveFromWorld();

    private IEnumerator TranslateAndFadeOut()
    {
        Vector3 spawnDirection = new Vector2(Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)).normalized;
        float moveSpeed = moveSpeedRange.RandomRangeValue;
        float spawnTime = Time.time;
        while (Time.time - spawnTime <= duration)
        {
            rTransform.Translate(spawnDirection * moveSpeed * Time.deltaTime);
            float percentageDuration = (Time.time - spawnTime) / duration;
            //damageText.alpha = (1f - Mathf.Pow(percentageDuration, 2)) * duration;
            SetAlpha(percentageDuration);
            yield return null;
        }
        RemoveFromWorld();
    }
}

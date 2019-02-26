using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour, IPoolable<DamageNumber>
{
    [SerializeField]
    private FloatRange moveSpeedRange = new FloatRange(1, 2);
    [SerializeField]
    private float duration = 1f;

    private RectTransform rTransform;
    private TextMeshPro damageText;

    public ObjectPool<DamageNumber> Pool { get; set; }

    private void Awake()
    {
        rTransform = GetComponent<RectTransform>();
        damageText = GetComponent<TextMeshPro>();
    }

    public void SpawnAtPos(Vector3 spawnPos, int damage)
    {
        transform.position = spawnPos;
        damageText.text = damage.ToString();
        StartCoroutine(TranslateAndFadeOut());
    }

    private IEnumerator TranslateAndFadeOut()
    {
        Vector3 spawnDirection = new Vector2(Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)).normalized;
        float moveSpeed = Random.Range(moveSpeedRange.Min, moveSpeedRange.Max);
        float spawnTime = Time.time;
        while (Time.time - spawnTime < duration)
        {
            rTransform.Translate(spawnDirection * moveSpeed * Time.deltaTime);
            float percentageDuration = (Time.time - spawnTime) / duration;
            damageText.alpha = (1f - Mathf.Pow(percentageDuration, 2)) * duration;
            yield return new WaitForEndOfFrame();
        }
        ReturnToPool();
    }

    public void ReturnToPool()
    {
        Pool.ReturnToPool(this);
    }
}

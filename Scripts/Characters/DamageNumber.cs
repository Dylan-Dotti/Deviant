using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour, IPoolable
{
    [SerializeField]
    DamageNumberPool returnPool;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float duration = 1f;

    private RectTransform rTransform;
    private TextMeshProUGUI damageText;

    private void Awake()
    {
        rTransform = GetComponent<RectTransform>();
        damageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SpawnAt(Vector3 spawnPos, int damage)
    {
        Debug.Log(damageText.alpha);
        damageText.alpha = 1f;
        transform.position = spawnPos;
        damageText.text = damage.ToString();
    }

    private IEnumerator TranslateAndFadeOut()
    {
        Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f),
            0f, Random.Range(-1f, 1f)).normalized;
        float spawnTime = Time.time;
        while (Time.time - spawnTime < duration)
        {
            rTransform.Translate(spawnDirection * moveSpeed);
            yield return null;
        }
        ReturnToPool();
    }

    public void ReturnToPool()
    {
        returnPool.ReturnToPool(this);
    }
}

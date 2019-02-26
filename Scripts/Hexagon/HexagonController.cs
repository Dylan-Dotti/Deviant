using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonController : MonoBehaviour
{
    [SerializeField]
    private Trapezoid trapezoidPrefab;
    [SerializeField]
    private float topDistFromCenter;
    [SerializeField]
    private float trapezoidHeight;
    [SerializeField]
    private float trapezoidCollapseSpeed;

    private Trapezoid[] trapezoids;

    private void Start()
    {
        trapezoids = new Trapezoid[5];
        SpawnTrapezoids();
    }

    private void Update()
    {
        if (topDistFromCenter > 0)
        {
            topDistFromCenter = Mathf.Max(0, topDistFromCenter - trapezoidCollapseSpeed * Time.deltaTime);
        }
        else if (trapezoidHeight > 0.01)
        {
            trapezoidCollapseSpeed *= 1.03f;
            trapezoidHeight = Mathf.Max(0.01f, trapezoidHeight - trapezoidCollapseSpeed * Time.deltaTime);
        }
        UpdateTrapezoids();
    }

    private void SpawnTrapezoids()
    {
        for (int i = 0; i < trapezoids.Length; i++)
        {
            Trapezoid newTz = Instantiate(trapezoidPrefab);
            newTz.transform.SetParent(transform);

            //init rotation
            float spawnAngle = 60 * i;
            newTz.transform.eulerAngles = new Vector3(newTz.transform.eulerAngles.x,
                newTz.transform.eulerAngles.y, spawnAngle + 90);

            //init position
            UpdateTrapezoid(newTz);
            trapezoids[i] = newTz;
        }
    }

    private void UpdateTrapezoids()
    {
        foreach (Trapezoid tz in trapezoids)
        {
            UpdateTrapezoid(tz);
        }
    }

    private void UpdateTrapezoid(Trapezoid tz)
    {
        tz.Height = trapezoidHeight;
        tz.TopLength = 1.9f * topDistFromCenter * Mathf.Tan(30 * Mathf.Deg2Rad);

        float angle = tz.transform.localEulerAngles.z - 90;
        float halfHeight = trapezoidHeight / 2f;
        float hypotenuse = topDistFromCenter + halfHeight;
        float xPos = hypotenuse * Mathf.Cos(angle * Mathf.Deg2Rad);
        float yPos = hypotenuse * Mathf.Sin(angle * Mathf.Deg2Rad);
        tz.transform.localPosition = new Vector2(xPos, yPos);
    }
}

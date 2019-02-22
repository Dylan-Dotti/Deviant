using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EquilateralTriangle : MonoBehaviour
{
    public float Height
    {
        get { return height; }
        set { height = value; UpdateMesh(); }
    }

    [SerializeField]
    private float height;

    private Mesh renderMesh;
    private MeshCollider colliderMesh;
    private Vector3[] vertices;
    private int[] triangles;

    private void Awake()
    {
        renderMesh = GetComponent<MeshFilter>().sharedMesh;
        colliderMesh = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        UpdateMesh();
    }

    private void Update()
    {
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        renderMesh.Clear();
        float halfHeight = height / 2f;
        float halfSideLength = height / Mathf.Tan(60 * Mathf.Deg2Rad);

        vertices = new Vector3[] { new Vector3(0, halfHeight),
                                   new Vector3(halfSideLength, -halfHeight),
                                   new Vector3(-halfSideLength, -halfHeight) };
        triangles = new int[] { 0, 1, 2 };
        renderMesh.vertices = vertices;
        renderMesh.triangles = triangles;
        renderMesh.RecalculateNormals();
        colliderMesh.sharedMesh = renderMesh;
    }
}

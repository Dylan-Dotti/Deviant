using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Trapezoid : MonoBehaviour
{
    public float Height { get { return height; } 
        set { height = value; } }
    public float TopLength { get { return topLength; }
        set { topLength = value; } }
    public float SideAngleDegrees { get { return sideAngleDegrees; }
        set { sideAngleDegrees = value; } }

    [SerializeField]
    private float height;
    [SerializeField]
    private float topLength;
    [SerializeField]
    private float sideAngleDegrees;

    private Mesh renderMesh;
    private MeshCollider colliderMesh;
    private Vector3[] vertices;
    private int[] triangles;

    
    private void Awake()
    {
        renderMesh = GetComponent<MeshFilter>().mesh;
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
        float halfLength = topLength / 2f;
        float halfHeight = height / 2f;
        float sideBotLength = height * Mathf.Tan(sideAngleDegrees * Mathf.Deg2Rad);
        float botLength = (2 * sideBotLength) + topLength;

        vertices = new Vector3[] { new Vector3(-halfLength, halfHeight),
                                   new Vector3(halfLength, halfHeight),
                                   new Vector3(-botLength / 2f, -halfHeight),
                                   new Vector3(-halfLength, -halfHeight),
                                   new Vector3(halfLength, -halfHeight),
                                   new Vector3(botLength / 2f, -halfHeight) };
        triangles = new int[] { 0, 1, 3,    1, 4, 3,
                                2, 0, 3,    5, 4, 1 };
        renderMesh.vertices = vertices;
        renderMesh.triangles = triangles;
        renderMesh.RecalculateNormals();
        colliderMesh.sharedMesh = renderMesh;
    }
}

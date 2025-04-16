using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;
public class DataViewBase
{
    #region RangeView


    [Header("----- RangeView -----")]
    [Range(0, 180)]
    public float angle = 30f;
    public float height = 1.0f;
    public float distance = 0f;
    public Color meshSightIn = Color.red;
    public Color meshSightOut = Color.red;
    public Mesh mesh;

    [Header("----- Owner ----- ")]
    public Transform Owner;


    #endregion
    [Header("----- DrawGizmo ----- ")]
    public bool IsDrawGizmo = false;

    [Header("----- LayerMask ----- ")]
    public LayerMask Scanlayers;
    public DataViewBase()
    { }
    public virtual bool IsInSight(Transform enemy)
    {
        return false;
    }
    public void CreateMesh()
    {
        mesh = CreateWedgeMesh();
    }
    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();
        int segments = 10;
        int numTriangles = (segments * 6) + 6;
        int numVertices = numTriangles * 3;
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.down * height;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance + Vector3.down * height;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance + Vector3.down * height;

        Vector3 topCenter = Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * (2 * height);
        Vector3 topRight = bottomRight + Vector3.up * (2 * height);

        int vert = 0;

        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance + Vector3.down * height;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance + Vector3.down * height;

            topRight = bottomRight + Vector3.up * (2 * height);
            topLeft = bottomLeft + Vector3.up * (2 * height);

            // far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // top 
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // bottom 
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    public virtual void OnDrawGizmos()
    {

    }
}

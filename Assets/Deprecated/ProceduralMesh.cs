﻿using UnityEngine;

public class ProceduralMesh : MonoBehaviour
{
    private float height;
    private int nbSides;

    // Outter shell is at radius1 + radius2 / 2, inner shell at radius1 - radius2 / 2
    private float bottomRadius1;

    private float bottomRadius2;
    private float topRadius1;
    private float topRadius2;

    private int nbVerticesCap;
    private int nbVerticesSides;

    private Mesh mesh;

    // bottom + top + sides
    private Vector3[] vertices;

    private int vert;
    private float _2pi;
    private Vector2[] uvs;

    // Bottom cap
    private int sideCounter;

    // bottom + top + sides
    private Vector3[] normales;

    private void Start()
    {
        height = 1f;
        nbSides = 24;

        // Outter shell is at radius1 + radius2 / 2, inner shell at radius1 - radius2 / 2
        bottomRadius1 = .5f;
        bottomRadius2 = .15f;
        topRadius1 = .5f;
        topRadius2 = .15f;

        nbVerticesCap = nbSides * 2 + 2;
        nbVerticesSides = nbSides * 2 + 2;

        // bottom + top + sides

        vertices = new Vector3[nbVerticesCap * 2 + nbVerticesSides * 2];

        vert = 0;

        _2pi = Mathf.PI * 2f;

        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
    }

    private void Vertices()
    {
        while (vert < nbVerticesCap)
        {
            sideCounter = sideCounter == nbSides ? 0 : sideCounter;

            float r1 = (float)(sideCounter++) / nbSides * _2pi;
            float cos = Mathf.Cos(r1);
            float sin = Mathf.Sin(r1);
            vertices[vert] = new Vector3(cos * (bottomRadius1 - bottomRadius2 * .5f), 0f, sin * (bottomRadius1 - bottomRadius2 * .5f));
            vertices[vert + 1] = new Vector3(cos * (bottomRadius1 + bottomRadius2 * .5f), 0f, sin * (bottomRadius1 + bottomRadius2 * .5f));
            vert += 2;
        }

        // Top cap
        sideCounter = 0;
        while (vert < nbVerticesCap * 2)
        {
            sideCounter = sideCounter == nbSides ? 0 : sideCounter;

            float r1 = (float)(sideCounter++) / nbSides * _2pi;
            float cos = Mathf.Cos(r1);
            float sin = Mathf.Sin(r1);
            vertices[vert] = new Vector3(cos * (topRadius1 - topRadius2 * .5f), height, sin * (topRadius1 - topRadius2 * .5f));
            vertices[vert + 1] = new Vector3(cos * (topRadius1 + topRadius2 * .5f), height, sin * (topRadius1 + topRadius2 * .5f));
            vert += 2;
        }

        // Sides (out)
        sideCounter = 0;
        while (vert < nbVerticesCap * 2 + nbVerticesSides)
        {
            sideCounter = sideCounter == nbSides ? 0 : sideCounter;

            float r1 = (float)(sideCounter++) / nbSides * _2pi;
            float cos = Mathf.Cos(r1);
            float sin = Mathf.Sin(r1);

            vertices[vert] = new Vector3(cos * (topRadius1 + topRadius2 * .5f), height, sin * (topRadius1 + topRadius2 * .5f));
            vertices[vert + 1] = new Vector3(cos * (bottomRadius1 + bottomRadius2 * .5f), 0, sin * (bottomRadius1 + bottomRadius2 * .5f));
            vert += 2;
        }

        // Sides (in)
        sideCounter = 0;
        while (vert < vertices.Length)
        {
            sideCounter = sideCounter == nbSides ? 0 : sideCounter;

            float r1 = (float)(sideCounter++) / nbSides * _2pi;
            float cos = Mathf.Cos(r1);
            float sin = Mathf.Sin(r1);

            vertices[vert] = new Vector3(cos * (topRadius1 - topRadius2 * .5f), height, sin * (topRadius1 - topRadius2 * .5f));
            vertices[vert + 1] = new Vector3(cos * (bottomRadius1 - bottomRadius2 * .5f), 0, sin * (bottomRadius1 - bottomRadius2 * .5f));
            vert += 2;
        }
    }

    private void Normales()
    {
        // bottom + top + sides
        sideCounter = 0;
        normales = new Vector3[vertices.Length];
        vert = 0;

        // Bottom cap
        while (vert < nbVerticesCap)
        {
            normales[vert++] = Vector3.down;
        }

        // Top cap
        while (vert < nbVerticesCap * 2)
        {
            normales[vert++] = Vector3.up;
        }

        // Sides (out)
        sideCounter = 0;
        while (vert < nbVerticesCap * 2 + nbVerticesSides)
        {
            sideCounter = sideCounter == nbSides ? 0 : sideCounter;

            float r1 = (float)(sideCounter++) / nbSides * _2pi;

            normales[vert] = new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1));
            normales[vert + 1] = normales[vert];
            vert += 2;
        }

        // Sides (in)
        sideCounter = 0;
        while (vert < vertices.Length)
        {
            sideCounter = sideCounter == nbSides ? 0 : sideCounter;

            float r1 = (float)(sideCounter++) / nbSides * _2pi;

            normales[vert] = -(new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1)));
            normales[vert + 1] = normales[vert];
            vert += 2;
        }
    }

    private void UVs()
    {
        uvs = new Vector2[vertices.Length];

        vert = 0;
        // Bottom cap
        sideCounter = 0;
        while (vert < nbVerticesCap)
        {
            float t = (float)(sideCounter++) / nbSides;
            uvs[vert++] = new Vector2(0f, t);
            uvs[vert++] = new Vector2(1f, t);
        }

        // Top cap
        sideCounter = 0;
        while (vert < nbVerticesCap * 2)
        {
            float t = (float)(sideCounter++) / nbSides;
            uvs[vert++] = new Vector2(0f, t);
            uvs[vert++] = new Vector2(1f, t);
        }

        // Sides (out)
        sideCounter = 0;
        while (vert < nbVerticesCap * 2 + nbVerticesSides)
        {
            float t = (float)(sideCounter++) / nbSides;
            uvs[vert++] = new Vector2(t, 0f);
            uvs[vert++] = new Vector2(t, 1f);
        }

        // Sides (in)
        sideCounter = 0;
        while (vert < vertices.Length)
        {
            float t = (float)(sideCounter++) / nbSides;
            uvs[vert++] = new Vector2(t, 0f);
            uvs[vert++] = new Vector2(t, 1f);
        }
    }

    private void Triangles()
    {
        int nbFace = nbSides * 4;
        int nbTriangles = nbFace * 2;
        int nbIndexes = nbTriangles * 3;
        int[] triangles = new int[nbIndexes];

        // Bottom cap
        int i = 0;
        sideCounter = 0;
        while (sideCounter < nbSides)
        {
            int current = sideCounter * 2;
            int next = sideCounter * 2 + 2;

            triangles[i++] = next + 1;
            triangles[i++] = next;
            triangles[i++] = current;

            triangles[i++] = current + 1;
            triangles[i++] = next + 1;
            triangles[i++] = current;

            sideCounter++;
        }

        // Top cap
        while (sideCounter < nbSides * 2)
        {
            int current = sideCounter * 2 + 2;
            int next = sideCounter * 2 + 4;

            triangles[i++] = current;
            triangles[i++] = next;
            triangles[i++] = next + 1;

            triangles[i++] = current;
            triangles[i++] = next + 1;
            triangles[i++] = current + 1;

            sideCounter++;
        }

        // Sides (out)
        while (sideCounter < nbSides * 3)
        {
            int current = sideCounter * 2 + 4;
            int next = sideCounter * 2 + 6;

            triangles[i++] = current;
            triangles[i++] = next;
            triangles[i++] = next + 1;

            triangles[i++] = current;
            triangles[i++] = next + 1;
            triangles[i++] = current + 1;

            sideCounter++;
        }

        // Sides (in)
        while (sideCounter < nbSides * 4)
        {
            int current = sideCounter * 2 + 6;
            int next = sideCounter * 2 + 8;

            triangles[i++] = next + 1;
            triangles[i++] = next;
            triangles[i++] = current;

            triangles[i++] = current + 1;
            triangles[i++] = next + 1;
            triangles[i++] = current;

            sideCounter++;
        }

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
    }
}
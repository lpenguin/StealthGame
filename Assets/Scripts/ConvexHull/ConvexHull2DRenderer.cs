using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ConvexHullCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class ConvexHull2DRenderer: MonoBehaviour
{
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private ConvexHullCollider2D _convexHullCollider;
    
    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _convexHullCollider = GetComponent<ConvexHullCollider2D>();
        
        var mesh = _meshFilter.sharedMesh;
        if (mesh == null)
        {
            CreateMesh();
        }
        
        if (_meshRenderer.sharedMaterial == null)
        {
            CreateMaterial();
        }
        
        _convexHullCollider.ConvexHullChanged.AddListener(OnConvexHullChanged);
    }

    private void OnConvexHullChanged()
    {
        UpdateMesh(_meshFilter.sharedMesh, _convexHullCollider.ConvexHull);
    }

    private void CreateMaterial()
    {
        #if UNITY_EDITOR
        _meshRenderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");
        #endif
    }

    private static void UpdateMesh(Mesh mesh, ConvexHull2D hull)
    {
        var points = hull.Points;
        
        Vector2 center = points[0];
        for (int i = 0; i < points.Count; i++)
        {
            center += points[i];
        }

        center /= points.Count;
        List<int> triangleIndices = new List<int>();
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(center.ToVector3());
        
        
        for (int i = 0; i < points.Count - 1; i++)
        {
            vertices.Add(points[i].ToVector3());
            triangleIndices.AddRange(new []{0, i + 2, i + 1});
        }

        int lastInd = points.Count - 1;
        
        vertices.Add(points[lastInd].ToVector3());
        triangleIndices.AddRange(new []{0, 0 + 1, lastInd + 1});
        
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangleIndices.ToArray();
    }

    private void CreateMesh()
    {
        var hull = _convexHullCollider.ConvexHull;
        var mesh = new Mesh();
        UpdateMesh(mesh, hull);
        _meshFilter.sharedMesh = mesh;
    }
}
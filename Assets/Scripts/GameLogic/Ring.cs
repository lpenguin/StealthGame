using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class Ring: MonoBehaviour
{
    [SerializeField]
    public float radius = 0.1f;

    [SerializeField]
    private float density = 30f; // num points per unit
    
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        float length = 2 * Mathf.PI * radius;
        int numPoints = (int) (density * length);

        float alphaStep = Mathf.PI * 2 / numPoints;
        Vector3[] points = new Vector3[numPoints + 1];
        for (int i = 0; i <= numPoints; i++)
        {
            float alpha = alphaStep * i;
            float x = Mathf.Cos(alpha) * radius;
            float y = Mathf.Sin(alpha) * radius;
            points[i] = new Vector3(x, 0, y); // XZ plane
        }

        _lineRenderer.positionCount = numPoints + 1;
        _lineRenderer.SetPositions(points);
    }
}
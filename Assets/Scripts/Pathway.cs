using UnityEngine;
using System;

public class Pathway : MonoBehaviour
{
    private LineRenderer _renderer;

    private void Start() {
        _renderer = GetComponent<LineRenderer>();
    }

    public Vector3 NextPoint(int index){
        int positionCount = _renderer.positionCount;
        int cyclePositionsCount = positionCount * 2 - 2;

        Vector3[] positions = new Vector3[cyclePositionsCount];
        _renderer.GetPositions(positions);

        Array.Copy(positions, 1, positions, positionCount, positionCount - 2);
        int cycleIndex = index % cyclePositionsCount;

        Debug.Log($"NextPoint({index}) translated to {cycleIndex} of {cyclePositionsCount}");

        return positions[cycleIndex];
    }
}

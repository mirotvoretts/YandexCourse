using System;
using System.Collections.Generic;
using UnityEngine;

public class FreeFall : MonoBehaviour, IMover
{
    private List<Vector3> _points;
    private int _pointsPerUnit;

    public Vector3 Evaluate(float time)
    {
        throw new System.NotImplementedException();
    }

    public void Throw(Vector3 target)
    {
    }

    private IEnumerable<Vector3> CalculateTrajectory(Vector3 start, Vector3 finish)
    {
        if (start == finish)
            throw new InvalidOperationException("Points mustn`t ve equal!");
        
        var points = Mathf.FloorToInt(Vector3.Distance(start, finish)) / _pointsPerUnit;

        for (int i = 0; i < points; i++)
        {
            yield return Vector3.Lerp(start, finish, (float)i / points);
        }
    }
}
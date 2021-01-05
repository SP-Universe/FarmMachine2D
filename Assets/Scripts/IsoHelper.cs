using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoHelper : MonoBehaviour
{
    public Vector3 IsoToXY(Vector3 v)
    {
        return new Vector3(0.5f * v.x - v.y, 0.5f * v.x + v.y, 0);
    }

    public Vector3 XYToIso(Vector3 v)
    {
        return new Vector3(v.x + v.y, 0.5f * (v.y - v.x),0);
    }
}

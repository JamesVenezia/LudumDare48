using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
    }

    public static Vector3 DirectionTo(this Vector3 original, Vector3 target)
    {
        return Vector3.Normalize(target - original);
    }

    public static Vector3 DirectionFrom(this Vector3 original, Vector3 target)
    {
        return Vector3.Normalize(original - target);
    }
}

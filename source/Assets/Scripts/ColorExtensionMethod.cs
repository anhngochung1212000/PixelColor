using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensionMethod
{
    public static bool CompareTwoColor(this Color a, Color b)
    {
        if (Mathf.Abs(a.r - b.r) > 0.05f || Mathf.Abs(a.g - b.g) > 0.05f || Mathf.Abs(a.b - b.b) > 0.05f)
            return false;//not equal

        return true;//equal
    }
}

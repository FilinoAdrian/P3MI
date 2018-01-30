using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBounds : MonoBehaviour
{
    private Bounds GetBoundsForAllChildren(GameObject findMyBounds)
    {
        Bounds result = new Bounds(Vector3.zero, Vector3.zero);

        foreach (var curRenderer in findMyBounds.GetComponentsInChildren<Renderer>())
        {
            if (result.extents == Vector3.zero)
            {
                result = curRenderer.bounds;
            }
            else
            {
                result.Encapsulate(curRenderer.bounds);
            }
        }

        return result;
    }
}

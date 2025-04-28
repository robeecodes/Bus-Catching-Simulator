using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInMainCameraView : MonoBehaviour
{
    public static bool IsInView(Vector3 target, Camera camera)
    {
        Vector3 viewPos = camera.WorldToViewportPoint(target);
        if (viewPos is { z: > 0, x: > 0 and < 1, y: > 0 and < 1 })
        {
            return true;
        }

        return false;
    }
}
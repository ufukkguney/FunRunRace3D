using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : Obstacle
{
    [HideInInspector]
    public Vector3 startPos, endPos;
    [HideInInspector]
    public float currentLerpTime = 0;
 
    private float lerpTime = 5;

    public float GetLerpTime()
    {
        return lerpTime;
    }
    [HideInInspector]
    public bool hit = false;

}

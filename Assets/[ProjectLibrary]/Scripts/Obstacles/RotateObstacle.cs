using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : Obstacle,IObstacle
{
    float yValue = 0;
    void Update()
    {
        ObstacleBehavior();
    }

    public void ObstacleBehavior()
    {
        yValue += Time.deltaTime * speed;
        Vector3 target = new Vector3(0, yValue, 0);
        transform.rotation = Quaternion.Euler(target);
    }

}

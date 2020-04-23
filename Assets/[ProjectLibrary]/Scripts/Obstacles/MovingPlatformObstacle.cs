using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformObstacle : MovingObstacle,IObstacle
{
    void Start()
    {
        startPos = new Vector3(transform.position.x -3, transform.position.y, transform.position.z);
        endPos = new Vector3(transform.position.x+3, transform.position.y, transform.position.z);

    }
    void Update()
    {
        ObstacleBehavior();
    }
    public void ObstacleBehavior()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime >= GetLerpTime())
        {
            currentLerpTime = GetLerpTime();
        }
        float Perc = currentLerpTime / GetLerpTime();

        if (!hit)
        {
            this.transform.position = Vector3.Lerp(startPos, endPos, Perc * speed);
            if (this.transform.position == endPos)
            {
                currentLerpTime = 0;
                hit = true;
            }
        }
        else if (hit)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, startPos, Perc * speed);
            if (this.transform.position == startPos)
            {
                currentLerpTime = 0;
                hit = false;
            }
        }
    }
}

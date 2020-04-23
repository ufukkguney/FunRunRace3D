using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezingObstacle : MovingObstacle, IObstacle
{
    private GameObject leftCube, rightCube;
    private Vector3 startPosright,endposright;
    
    void Start()
    {
        leftCube = transform.Find("LeftCube").gameObject;
        rightCube = transform.Find("RightCube").gameObject;

        startPos = leftCube.transform.position;
        startPosright = rightCube.transform.position;
        endPos = new Vector3(leftCube.transform.position.x + 2f, leftCube.transform.position.y, leftCube.transform.position.z);
        endposright = new Vector3(rightCube.transform.position.x - 2f, leftCube.transform.position.y, leftCube.transform.position.z);
    }
    private void Update()
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
            leftCube.transform.position = Vector3.Lerp(startPos, endPos, Perc*speed);
            rightCube.transform.position = Vector3.Lerp(startPosright, endposright, Perc*speed);
            if (leftCube.transform.position == endPos)
            {
                currentLerpTime = 0;
                hit = true;

            }
        }
        else if (hit)
        {
            leftCube.transform.position = Vector3.Lerp(leftCube.transform.position, startPos, Perc*speed);
            rightCube.transform.position = Vector3.Lerp(rightCube.transform.position, startPosright, Perc*speed);
            if (leftCube.transform.position == startPos)
            {
                currentLerpTime = 0;
                hit = false;
            }
        }
    }
}

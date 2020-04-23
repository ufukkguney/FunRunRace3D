using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class LevelBuilder : MonoBehaviour
{
    public GameObject gameCanvas;
    private Button levelfinishScreen;

    public GameObject startPlatformPrefab, middlePlatformPrefab, endPlatfromPrefab, movingPlatformPrefab;
    public List<GameObject> obstacles = new List<GameObject>();

    private int iterationRangeMin = 5;
    private int iterationRangeMax = 7;

    private float iterationValuePlatform = 0;
    
    private float obstacleSpawnThreshold = 2;

    [HideInInspector]
    public Vector3 endposforAI;
    
    void Awake()
    {
        MF_AutoPool.InitializeSpawn(startPlatformPrefab, 1, 100);
        MF_AutoPool.InitializeSpawn(middlePlatformPrefab, 1, 100);
        MF_AutoPool.InitializeSpawn(endPlatfromPrefab, 1, 100);
        MF_AutoPool.InitializeSpawn(movingPlatformPrefab, 4, 100);
        for (int i = 0; i < obstacles.Count; i++)
        {
            MF_AutoPool.InitializeSpawn(obstacles[i],1,50);
        }

        levelfinishScreen = gameCanvas.transform.Find("LevelFinishButton").GetComponent<Button>();
    }
    public static OnCoolGuySpawn onCoolGuySpa;
    public delegate void OnCoolGuySpawn();

    public static OnHoldEndPos OnHoldEndPosforAI;
    public delegate void OnHoldEndPos(Vector3 holdEndPos);

    void Start()
    {
        StartCoroutine("GenerateLevel");

        levelfinishScreen.onClick.AddListener(() => { NextLevel(); });
    }

    public void NextLevel()
    {
        iterationValuePlatform = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Camera>() == null)
            {
                Debug.Log(transform.GetChild(i).gameObject.name);

                Destroy(transform.GetChild(i).gameObject);
            }
           
        }

        //it for harder game than previous level
        iterationRangeMin += 5;
        iterationRangeMax += 5;

        obstacleSpawnThreshold -= 0.5f;
        if (obstacleSpawnThreshold < 1)
            obstacleSpawnThreshold = 1;
        //if obstacle speed increase, the level happened too hard

        StartCoroutine("GenerateLevel");
    }

    IEnumerator GenerateLevel()
    {
        levelfinishScreen.gameObject.SetActive(false);
        int obstacleSpawnControlCounter = 0;
        WaitForSeconds start = new WaitForSeconds(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();
        yield return start;

        //place for start platform
        SpawnPlatformandObstacle(startPlatformPrefab, false);
        onCoolGuySpa?.Invoke();
        yield return interval;

        //random iteration obstacles and plaforms
        int iterations = Random.Range(iterationRangeMin, iterationRangeMax);


        for (int i = 0; i < iterations; i++)
        {
            //place random from List
            int randomforMidleorMovingPlatform = UnityEngine.Random.Range(0, 10);
            //Debug.Log("randomForMovingPlatform : " + randomforMidleorMovingPlatform);

            if (randomforMidleorMovingPlatform < 9)
            {
                if (obstacleSpawnControlCounter >= obstacleSpawnThreshold)
                {
                    obstacleSpawnControlCounter = 0;
                    int randomforObstacles = Random.Range(0, obstacles.Count);
                    SpawnPlatformandObstacle(obstacles[randomforObstacles],true);
                    yield return interval;
                }
                else
                    obstacleSpawnControlCounter++;

                SpawnPlatformandObstacle(middlePlatformPrefab, false);
                yield return interval;

            }
            else
            {
                obstacleSpawnControlCounter = 0;
                SpawnPlatformandObstacle(movingPlatformPrefab, false);

            }
            yield return interval;
            
        }

        //place end platform
        SpawnPlatformandObstacle(endPlatfromPrefab,false);

        yield return interval;
        

        endposforAI = new Vector3(0, 10, iterationValuePlatform);

        OnHoldEndPosforAI?.Invoke(endposforAI);

        //Level generation finished
        ResetLevelGenerator();

    }

    private float IterationPositionValue(GameObject gameObject)
    {
        //Debug.Log("EndPoint : " + gameObject.transform.localScale.z/2);

         return gameObject.transform.position.z + (gameObject.transform.localScale.z/2);
    }

    void SpawnPlatformandObstacle(GameObject platformPrefab, bool isObstacle)
    {
        GameObject singleplatform = MF_AutoPool.Spawn(platformPrefab);

        singleplatform.transform.parent = this.transform;
        singleplatform.transform.position = Vector3.zero;
        
        singleplatform.transform.position = new Vector3(singleplatform.transform.position.x, singleplatform.transform.position.y, (singleplatform.transform.localScale.z / 2) + iterationValuePlatform);

        if (!isObstacle)
            iterationValuePlatform = IterationPositionValue(singleplatform);
        
    }
    
    void ResetLevelGenerator()
    {
        StopCoroutine("GenerateLevel");
    }
}

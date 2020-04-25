using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveWithAI : MonoBehaviour
{
    public GameObject gameCanvas;
    public NavMeshAgent agent;
    public GameObject coolguy;
    private GameObject coolguyOnGame;
    private GameObject mainCamera;
    private Animator animatorController;
    private Button levelfinishScreen;

    private bool isFinish = false;

    private Vector3 target;

    public NavMeshSurface surface;
    

    void Start()
    {
        mainCamera = transform.Find("Main Camera").gameObject;
        levelfinishScreen = gameCanvas.transform.Find("LevelFinishButton").GetComponent<Button>();
        gameCanvas.SetActive(false);

        LevelBuilder.onCoolGuySpa += null;
        LevelBuilder.onCoolGuySpa += SpawnCoolGuy; 
        LevelBuilder.OnHoldEndPosforAI += MovewithAI;
        CoolGuy.CollisionTypeSnd += null;
        CoolGuy.CollisionTypeSnd += CollisionBehaviour;
    }


    public void CollisionBehaviour(GameObject collisionObject)
    {
        Debug.Log(collisionObject.gameObject.name);

        if (collisionObject.gameObject.GetComponent<Platform>() != null)//Control on which platform
        {
            if (collisionObject.gameObject.name.Contains("End"))
            {
                isFinish = true;
                animatorController.SetFloat("vertical", 0);
                gameCanvas.SetActive(true);
                Debug.Log(collisionObject.gameObject.name);
                levelfinishScreen.gameObject.SetActive(true);
            }
        }

    }



    private void Update()
    {
        if (target != null && agent != null && !isFinish)
        {
            agent.Move(target * Time.deltaTime / 10);
            animatorController.SetFloat("vertical", 1);
        }
        surface.BuildNavMesh();
    }


    public void MovewithAI(Vector3 endposforAI)
    {
        target = endposforAI;
    }


    public void SpawnCoolGuy()
    {
        MF_AutoPool.InitializeSpawn(coolguy, 1, 50);

        coolguyOnGame = MF_AutoPool.Spawn(coolguy);

        coolguyOnGame.transform.position = new Vector3(0, 5, 1);
        coolguyOnGame.transform.parent = transform;
        CameraControl();
        animatorController = coolguyOnGame.GetComponent<Animator>();
        StartCoroutine("HoldNavAgent");
    }
    private void CameraControl()
    {
        mainCamera.transform.parent = coolguyOnGame.transform;
        mainCamera.transform.position = Vector3.zero;
        mainCamera.transform.position = new Vector3(0, 8, -6);
        mainCamera.transform.rotation = Quaternion.Euler(20, 0, 0);
    }

    public IEnumerator HoldNavAgent()
    {
        yield return new WaitForSeconds(0.1f);

        coolguyOnGame.GetComponent<NavMeshAgent>().enabled = true;

        agent = coolguyOnGame.GetComponent<NavMeshAgent>();
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveWithAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject coolguy;
    private GameObject coolguyOnGame;
    private GameObject mainCamera;

    public GameObject Gamecanvas;

    private Vector3 target;

    public NavMeshSurface surface;


    void Start()
    {
        mainCamera = transform.Find("Main Camera").gameObject;

        Gamecanvas.SetActive(false);

        LevelBuilder.onCoolGuySpa += null;

        LevelBuilder.onCoolGuySpa += SpawnCoolGuy; 

        LevelBuilder.OnHoldEndPosforAI += MovewithAI;


    }
    private void Update()
    {
        if (target != null && agent != null)
        {
            Debug.Log("Target Destination : " + target);
            agent.Move(target * Time.deltaTime/10);
        }

        surface.BuildNavMesh();

    }


    public void MovewithAI(Vector3 endposforAI)
    {
        Debug.Log("Target Destination : " + endposforAI/10);
        target = endposforAI;

        //agent.SetDestination(target);
    }


    public void SpawnCoolGuy()
    {
        MF_AutoPool.InitializeSpawn(coolguy, 1, 50);

        coolguyOnGame = MF_AutoPool.Spawn(coolguy);

        coolguyOnGame.transform.position = new Vector3(0, 5, 1);
        coolguyOnGame.transform.parent = transform;
        CameraControl();

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

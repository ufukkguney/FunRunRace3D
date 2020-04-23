using UnityEngine;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public GameObject gameCanvas;
    private Button levelfinishScreen;
    public GameObject coolguy;
    private Animator animatorController;
    private GameObject mainCamera;
    private GameObject coolguyOnGame;


    private Collider mainCollider;
    private Collider[] allColliders;

    private int lifeAmount = 3;

    private Vector3 holdPosCheckpoint, holdStartPos;

    private Text lifeAmountText;

    private float waittimeforspawn = 1;

    void Awake()
    {
        mainCamera = transform.Find("Main Camera").gameObject;
        levelfinishScreen = gameCanvas.transform.Find("LevelFinishButton").GetComponent<Button>();
        lifeAmountText = gameCanvas.transform.Find("LifeAmountText").GetComponent<Text>();
    }


    void Start()
    {
        MF_AutoPool.InitializeSpawn(coolguy, 1, 50);
       
        //events for character control
        CoolGuy.CollisionTypeSnd += null;
        LevelBuilder.onCoolGuySpa += null;
        LevelBuilder.onCoolGuySpa += SpawnCoolGuy;
        CoolGuy.CollisionTypeSnd += CollisionBehaviour;
        CoolGuy.CollisionCountSnd += GameFailedFromFallPlatform;

    
        lifeAmountText.text = "lifeAmount : " + lifeAmount.ToString();
    }


    public void SpawnCoolGuy()
    {
        coolguyOnGame = MF_AutoPool.Spawn(coolguy);

        coolguyOnGame.transform.position = new Vector3(0, 3, 1);
        coolguyOnGame.transform.parent = transform;
        animatorController = coolguyOnGame.GetComponent<Animator>();

        CameraControl();
        //getting collider for ragdoll
        mainCollider = coolguyOnGame.transform.GetComponent<Collider>();
        allColliders = coolguyOnGame.transform.GetChild(0).GetComponentsInChildren<Collider>(true);

        //holding start position
        holdStartPos = coolguyOnGame.transform.position;

        DoRagdoll(false);
    }

    public void DoRagdoll(bool isRagdoll)
    {
        mainCollider.enabled = !isRagdoll;

        foreach (var col in allColliders)
            col.enabled = isRagdoll;

        coolguyOnGame.GetComponent<Rigidbody>().useGravity = !isRagdoll;
        coolguyOnGame.GetComponent<Animator>().enabled = !isRagdoll;

        if (lifeAmount > 0 && isRagdoll)//check if is there chance
        {
            //place where last position
            StartCoroutine("ReSpawn");
            lifeAmount--;
            lifeAmountText.text = "lifeAmount : " + lifeAmount.ToString();
        }
        else if (lifeAmount == 0)
        {
            StartCoroutine("RestartGame");
        }
    }

    private void CameraControl()
    {
        mainCamera.transform.parent = coolguyOnGame.transform;
        mainCamera.transform.position = Vector3.zero;
        mainCamera.transform.position = new Vector3(0, 8, -6);
        mainCamera.transform.rotation = Quaternion.Euler(20, 0, 0);
    }

    public void CollisionBehaviour(GameObject collisionObject)
    {
        
        if (collisionObject.gameObject.GetComponent<Obstacle>() != null)
        {
            if (collisionObject.gameObject.name.Contains("Platform"))//control is on moving platform
            {
                coolguyOnGame.transform.parent = collisionObject.transform;
            }
            else //place where we will do ragdoll
            {
                Shader shadernoise = Shader.Find("Shader Graphs/TempShaderGraph");

                collisionObject.transform.GetComponent<Renderer>().material.shader = shadernoise;

                DoRagdoll(true);

                StartCoroutine("FixObstacleShader", collisionObject.gameObject);
            }
        }
        else if (collisionObject.gameObject.GetComponent<Platform>() != null)//Control on which platform
        {
            holdPosCheckpoint = coolguyOnGame.transform.position - new Vector3(0, 0, 1);

            coolguyOnGame.transform.parent = collisionObject.transform;

            if (collisionObject.gameObject.name.Contains("End"))
            {
                mainCamera.transform.parent = this.transform;
                levelfinishScreen.gameObject.SetActive(true);
                lifeAmount = 3;
                lifeAmountText.text = "lifeAmount : " + lifeAmount.ToString();
            }
        }
        
    }

    IEnumerator ReSpawn()
    {
        yield return new WaitForSeconds(waittimeforspawn);

        coolguyOnGame.transform.position = holdPosCheckpoint;

        //yield return new WaitForSeconds(0.3f);

        DoRagdoll(false);

    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(waittimeforspawn);

        lifeAmount = 3;
        lifeAmountText.text = "lifeAmount : " + lifeAmount.ToString();
        coolguyOnGame.transform.position = holdStartPos;
    }

    IEnumerator FixObstacleShader(GameObject obstacleobject)
    {
        yield return new WaitForSeconds(waittimeforspawn);
        Shader mainShader = Shader.Find("Universal Render Pipeline/Nature/SpeedTree7");
        obstacleobject.GetComponent<Renderer>().material.shader = mainShader;
    }


    public void GameFailedFromFallPlatform(int collisionCount)
    {
        if (collisionCount == 0)
        {
            DoRagdoll(true);
        }
    }


    public void CharacterMovementUP()
    {
        animatorController.SetFloat("vertical",0);
    }

    public void CharacterMovementDown()
    {
        animatorController.SetFloat("vertical", 1);
    }
}
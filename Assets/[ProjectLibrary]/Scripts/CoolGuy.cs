using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolGuy : MonoBehaviour
{
    public static CollisionTypeSend CollisionTypeSnd;
    public delegate void CollisionTypeSend(GameObject collisionObject);

    public int collisionCount = 0;

    public static CollisionCountSend CollisionCountSnd;
    public delegate void CollisionCountSend(int collisionCount);
    
    void OnCollisionEnter(Collision collision)
    {
        collisionCount++;
        CollisionTypeSnd?.Invoke(collision.gameObject);
        //Debug.Log("Collision Name : " + collision.gameObject.name + " CollisionCount : " + collisionCount);
    }
    void OnCollisionExit(Collision collision)
    {
        collisionCount--;
        CollisionCountSnd?.Invoke(collisionCount);
        //Debug.Log("Log From Exit : " + collision.gameObject.name + " CollisionCount : " + collisionCount);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveBlock : MonoBehaviour
{
    public static StackEvent stackEvent;
    public static UnstackEvent unstackEvent;
    public static PickUpEvent pickUpEvent;
    public static PutDownEvent putDownEvent;
    public float speed;
    private bool stacking = false;
    //private bool unstacking = false;
    private bool pickingUp = false;
    private bool falling = false;
    private GameObject bottomBlock;
    private GameObject topBlock;
    private float blockHeight;
    private bool moveRight;
    private Rigidbody topRigidBody; 
    private Rigidbody bottomRigidBody;


    public class StackEvent : UnityEvent<string, string>
    {
    }
    public class UnstackEvent : UnityEvent<string, string>
    {
    }
    public class PickUpEvent : UnityEvent<string>
    {
    }
    public class PutDownEvent : UnityEvent<string>
    {
    }

    void Start()
    {
        stackEvent = new StackEvent();
        unstackEvent = new UnstackEvent();
        pickUpEvent = new PickUpEvent();
        putDownEvent = new PutDownEvent();
        stackEvent.AddListener(StackBlock);
        unstackEvent.AddListener(UnstackBlock);
        pickUpEvent.AddListener(PickUpBlock);
        putDownEvent.AddListener(PutDownBlock);
    }
    private void StackBlock(string top, string bottom)
    {
        Debug.Log("stacking " + top + " on " + bottom);
        stacking = true;
        topBlock = GameObject.FindGameObjectWithTag(top);
        bottomBlock = GameObject.FindGameObjectWithTag(bottom);
        bottomRigidBody = bottomBlock.GetComponent<Rigidbody>();

        moveRight = false;
        if (topBlock.transform.position.x < bottomBlock.transform.position.x)
        {
            moveRight = true;
        }
    }

    private void UnstackBlock(string top, string bottom)
    {
        Debug.Log("unstacking " + top + " from " + bottom);
        topBlock = GameObject.FindGameObjectWithTag(top);
        PickUpBlock(top);
        
    }
    private void PickUpBlock(string block)
    {
        Debug.Log("picking up " + block);
        pickingUp = true;
        topBlock = GameObject.FindGameObjectWithTag(block);
        blockHeight = topBlock.GetComponent<MeshCollider>().bounds.extents.y;
        topRigidBody = topBlock.GetComponent<Rigidbody>();
        topRigidBody.useGravity = false;
    }

    private void PutDownBlock(string block)
    {
        Debug.Log("putting down " + block);
        ExecutePlan.actionFinished.Invoke();
    }

    private void Update()
    {
        if(pickingUp)
        {
            if (topBlock.transform.position.y < blockHeight * 10)
            {
                topBlock.transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            else
            {
                pickingUp = false;
                ExecutePlan.actionFinished.Invoke();
            }

        }
        else if(stacking)
        {
            if(!falling)
            {
                if (moveRight)
                {
                    if (topBlock.transform.position.x < bottomBlock.transform.position.x)
                    {
                        topBlock.transform.Translate(Vector3.right * speed * Time.deltaTime);
                    }
                    else
                    {
                        falling = true;
                        topRigidBody.useGravity = true;
                    }
                }
                else
                {
                    if (topBlock.transform.position.x > bottomBlock.transform.position.x)
                    {
                        topBlock.transform.Translate(Vector3.right * speed * Time.deltaTime * -1);
                    }
                    else
                    {
                        falling = true;
                        topRigidBody.useGravity = true;
                    }
                }
            }
            else
            {
                //if colliding with bottom block
                if (topBlock.transform.position.y <= bottomBlock.transform.position.y + 2)
                {
                    falling = false;
                    stacking = false;
                    ExecutePlan.actionFinished.Invoke();
                }
            }
        }
    }
}

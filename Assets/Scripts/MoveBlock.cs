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
    private bool pickingUp = false;
    private bool falling = false;
    private bool puttingDown = false;
    private GameObject bottomBlock;
    private GameObject topBlock;
    private float blockSize;
    private float bottomBlockHeight;
    private float boundary;
    private bool moveRight;
    private Rigidbody topRigidBody;

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
        blockSize = GameObject.FindGameObjectWithTag("red").GetComponent<MeshCollider>().bounds.extents.y * 2;
    }
    private void StackBlock(string top, string bottom)
    {
        GameManager.stateText.text = "stacking " + top + " on " + bottom;
        topBlock = GameObject.FindGameObjectWithTag(top);
        bottomBlock = GameObject.FindGameObjectWithTag(bottom);
        boundary = bottomBlock.transform.position.x;
        moveRight = false;
        if (topBlock.transform.position.x < boundary)
        {
            moveRight = true;
        }
        stacking = true;
    }

    private void UnstackBlock(string top, string bottom)
    {
        GameManager.stateText.text = "unstacking " + top + " from " + bottom;
        topBlock = GameObject.FindGameObjectWithTag(top);
        PickUpBlock(top);
        
    }
    private void PickUpBlock(string block)
    {
        GameManager.stateText.text = "picking up " + block;
        topBlock = GameObject.FindGameObjectWithTag(block);
        
        bottomBlockHeight = blockSize * 10;
        topRigidBody = topBlock.GetComponent<Rigidbody>();
        topRigidBody.useGravity = false;
        pickingUp = true;
    }

    private void PutDownBlock(string block)
    {
        GameManager.stateText.text = "putting down " + block;
        topBlock = GameObject.FindGameObjectWithTag(block);
        //topBlock.transform.localScale = GameManager.scale;
        moveRight = false;
        boundary = GameManager.blockPositions[block].x;
        if (topBlock.transform.position.x < boundary)
        {
            moveRight = true;
        }
        puttingDown = true;
    }

    private void Update()
    {
        if(pickingUp)
        {
            if (topBlock.transform.position.y < bottomBlockHeight)
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
                    if (topBlock.transform.position.x < boundary)
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
                    if (topBlock.transform.position.x > boundary)
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
        else if(puttingDown)
        {
            if (!falling)
            {
                if (moveRight)
                {
                    if (topBlock.transform.position.x < boundary)
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
                    if (topBlock.transform.position.x > boundary)
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
                if (topBlock.transform.position.y <= 0)
                {
                    falling = false;
                    puttingDown = false;
                    ExecutePlan.actionFinished.Invoke();
                }
            }
        }
    }
}

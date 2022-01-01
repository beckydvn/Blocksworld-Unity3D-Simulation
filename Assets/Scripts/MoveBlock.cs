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
    private bool unstacking = false;
    private bool pickingUp = false;
    private bool puttingDown = false;
    private GameObject bottomBlock;
    private GameObject topBlock;
    private float blockHeight;

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
        ExecutePlan.actionFinished.Invoke();
    }

    private void UnstackBlock(string top, string bottom)
    {
        Debug.Log("unstacking " + top + " from " + bottom);
        ExecutePlan.actionFinished.Invoke();
    }
    private void PickUpBlock(string block)
    {
        Debug.Log("picking up " + block);
        pickingUp = true;
        bottomBlock = GameObject.FindGameObjectWithTag(block);
        blockHeight = bottomBlock.GetComponent<MeshCollider>().bounds.extents.y;
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
            if (bottomBlock.transform.position.y < blockHeight * 10)
            {
                bottomBlock.transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            else
            {
                pickingUp = false;
                ExecutePlan.actionFinished.Invoke();
            }
        }
    }
}

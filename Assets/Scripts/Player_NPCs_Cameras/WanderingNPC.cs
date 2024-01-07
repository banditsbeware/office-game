using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingNPC : MonoBehaviour
{
    internal Transform thisTransform;
    public float moveSpeed = 0.6f;
 
    // A minimum and maximum time delay for taking a decision, choosing a direction to move in
    public Vector2 decisionTime = new Vector2(2, 10);
    internal float decisionTimeCount = 0;
 
    internal Vector3[] moveDirections = new Vector3[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back, Vector3.zero, Vector3.zero };
    internal int currentMoveDirection;
 
    void Start()
    {
        thisTransform = this.transform;

        // Set a random time delay for taking a decision ( changing direction, or standing in place for a while )
        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
 
        // Choose a movement direction, or stay in place
        ChooseMoveDirection();
    }
 
    // Update is called once per frame
    void Update()
    {
        // Move the object in the chosen direction at the set speed
        thisTransform.position += moveDirections[currentMoveDirection] * Time.deltaTime * moveSpeed;
 
       
        if (decisionTimeCount > 0) 
        {
            decisionTimeCount -= Time.deltaTime;
        }
        else
        {
            decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
            ChooseMoveDirection();
        }
    }
 
    void ChooseMoveDirection()
    {
        // Choose whether to move sideways or up/down
        currentMoveDirection = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
    }

    //called from child Personal Bubble, sends the Collider that the bubble entered
    public void BubbleEntered(Collider2D other) {
        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
        currentMoveDirection = 5; //zero
    }
}

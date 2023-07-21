using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SwipeMove : MonoBehaviour
{
    public float decel = -0.3f;
    public float accel = 0.8f;

    public Vector2 targetPosition;

    public float velocity = 0f;
    public bool moving = false;
    public float timeMoving = 0f;
    public float accelTime = 1f;

    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving & velocity <= 0f)
        {
            Debug.Log("Staying!");
            velocity = 0f;
            moving = false;
        }
        else if (moving)
        {
            Debug.Log("Moving!");
            float distance;
            if (timeMoving > accelTime)
            {
                Debug.Log("Decel");
                float decelTime = timeMoving - accelTime;
                distance = (accel * decelTime) + (.5f * accel * (decelTime * decelTime));
            }
            else
            {
                Debug.Log("Accel");
                distance = (.05f * accel * (timeMoving * timeMoving));
            }
            Debug.Log("Distance: " + distance);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, distance);
            timeMoving += Time.deltaTime;

        }


    }
}

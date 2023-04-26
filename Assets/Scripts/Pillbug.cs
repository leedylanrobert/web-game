using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillbug : MonoBehaviour
{

    Vector2 targetPosition;

    public Entry entry;
    private bool startMoving = false;

    private bool cooldown = false;
    // private bool skipCooldown = true;
    private int cooldownCount = 0;
    private int cooldownTime = 300;    

    private float xMax;
    private float yMax;

    float speed;

    public float secondsToMaxDifficulty;

    public float minSpeed;
    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 topRightWorld = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        xMax = topRightWorld.x;
        yMax = topRightWorld.y;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (startMoving)
        {
            if (transform.position.x <= -xMax | transform.position.x >= xMax | transform.position.y <= -yMax | transform.position.y >= yMax)
            {
                if (cooldownCount >= cooldownTime)
                {
                    SetTargetPosition();
                    cooldown = false;
                }
                else
                {
                    cooldown = true;
                    cooldownCount += 1;
                }
            }
            else
            {
                cooldownCount = 0;
            }
            if (!cooldown)
            {
                speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
        else if (entry.isSpawned == true)
        {
            startMoving = true;
            SetTargetPosition();
            cooldownCount = cooldownTime;
        }
    }

    void SetTargetPosition()
    {
        Vector2 spiderPosition = GameObject.Find("Spider").transform.position;

        float slope = (spiderPosition.y - transform.position.y) / (spiderPosition.x - transform.position.x);

        // Right side intersection
        float xCrossRight = xMax - transform.position.x;
        float yCrossRight = slope * xCrossRight;
        float cRight = Mathf.Sqrt((xCrossRight * xCrossRight) + (yCrossRight * yCrossRight));

        // Top side intersection
        float yCrossTop = yMax - transform.position.x;
        float xCrossTop = yCrossTop / slope;
        float cTop = Mathf.Sqrt((xCrossTop * xCrossTop) + (yCrossTop * yCrossTop));

        // Left side intersection
        float xCrossLeft = Mathf.Abs(-xMax - transform.position.x);
        float yCrossLeft = Mathf.Abs(slope * xCrossLeft);
        float cLeft = Mathf.Sqrt((xCrossLeft * xCrossLeft) + (yCrossLeft * yCrossLeft));

        // Bottom side intersection
        float yCrossBottom = Mathf.Abs(-yMax - transform.position.x);
        float xCrossBottom = Mathf.Abs(yCrossBottom / slope);
        float cBottom = Mathf.Sqrt((xCrossBottom * xCrossBottom) + (yCrossBottom * yCrossBottom));

        // If pillbug position is origin, which quadrant is spider in?
        if (spiderPosition.x > transform.position.x & spiderPosition.y > transform.position.y)
        // Quadrant I, infinite line will intersect top and right sides
        {
            if (cRight < cTop)
            {
                targetPosition = new Vector2(xMax, transform.position.y + yCrossRight);
            }
            else
            {
                targetPosition = new Vector2(transform.position.x + xCrossTop, yMax);
            }
        }
        else if (spiderPosition.x < transform.position.x & spiderPosition.y > transform.position.y)
        // Quadrant II
        {
            if (cLeft < cTop)
            {
                targetPosition = new Vector2(-xMax, transform.position.y + yCrossLeft);
            }
            else
            {
                targetPosition = new Vector2(transform.position.x - xCrossTop, yMax);
            }
        }
        else if (spiderPosition.x < transform.position.x & spiderPosition.y < transform.position.y)
        // Quadrant III
        {
            if (cLeft < cBottom)
            {
                targetPosition = new Vector2(-xMax, transform.position.y - yCrossLeft);
            }
            else
            {
                targetPosition = new Vector2(transform.position.x - xCrossBottom, -yMax);
            }
        }
        else
        // Quadrant IV
        {
            if (cRight < cBottom)
            {
                targetPosition = new Vector2(xMax, transform.position.y - yCrossRight);
            }
            else
            {
                targetPosition = new Vector2(transform.position.x + xCrossBottom, -yMax);
            }  
        }
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

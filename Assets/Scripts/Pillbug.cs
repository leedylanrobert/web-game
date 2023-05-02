using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillbug : MonoBehaviour
{

    Vector2 targetPosition;

    public Entry entry;
    private bool startMoving = false;

    private bool cooldown = false;
    private float cooldownCount = 0f;
    private float cooldownTime = 1.5f;    

    private float xMax;
    private float yMax;

    float speed;

    public float secondsToMaxDifficulty;

    public float minSpeed;
    public float maxSpeed;

    public EnemyProps enemyProps;

    Vector2 startingPosition;

    float acceleration = .0012f;
    // float acceleration = .0175f;
    float deceleration = 0f;
    float timeCount = 0f;
    float totalDistance = 0f;

    bool recoil = false;
    int recoilDirection = -1;

    float achievedSpeed = 0f;
    float achievedTime = 0f;
    float achievedDistance = 0f;
    float lastDistance = 0f;
    float retargetDiff = 0f;
    // int direction guide: 0: down, 1: up, 2: left, 3: right

    public AudioClip strike;
    public AudioClip bowling;
    AudioSource audioSource;
    float volume = 0.6f;

    float slope;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 topRightWorld = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        xMax = topRightWorld.x;
        yMax = topRightWorld.y;

        startingPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("TimeCount: " + timeCount);
        Move();
    }

    void Move()
    {
        if (startMoving)
        {
            if ((transform.position.x - transform.localScale.x <= -xMax | transform.position.x + transform.localScale.x >= xMax | transform.position.y - transform.localScale.y <= -yMax | transform.position.y + transform.localScale.y >= yMax) 
            & enemyProps.isOnScreen)
            // Pillbug has hit screen boundary
            {
                if (recoil)
                {
                    bool newCollision = CheckRecoilDirection();
                    if (newCollision)
                    {
                        SetRecoilDirection();
                        startingPosition = transform.position;
                        retargetDiff = lastDistance;
                    }
                }
                else
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(strike, volume * .5f);
                    float xDiff = Mathf.Abs(transform.position.x - startingPosition.x);
                    float yDiff = Mathf.Abs(transform.position.y - startingPosition.y);
                    achievedDistance = Mathf.Sqrt((xDiff * xDiff) + (yDiff * yDiff));

                    startingPosition = transform.position;
                    cooldown = true;

                    achievedSpeed = (achievedDistance - ((acceleration * (timeCount * timeCount)) / 2)) / timeCount;
                    achievedTime = timeCount;
                    timeCount = 0f;

                    recoil = true;

                    CheckRecoilDirection();
                }
            }
            if (!cooldown)
            {
                if (!enemyProps.isOnScreen & (transform.position.x - transform.localScale.x > -xMax & transform.position.x + transform.localScale.x < xMax & transform.position.y - transform.localScale.y > -yMax & transform.position.y + transform.localScale.y < yMax))
                // If pillbug makes it onto the screen update the enemyProps
                {
                    enemyProps.isOnScreen = true;
                }
                float distance = acceleration * (timeCount * timeCount) / 2;
                // speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, distance);
                // transform.Rotate(Vector3.forward * (.45f + (totalDistance * .05f)));
                transform.Rotate((Vector3.forward * (.45f + (totalDistance * .05f))));

                totalDistance += distance;
                timeCount += Time.deltaTime;
            }
            else
            {
                timeCount += Time.deltaTime;
                if (recoil)
                {
                    if (deceleration == 0f)
                    {
                        float a = xMax * 2;
                        float b = yMax * 2;
                        float maxDistance = Mathf.Sqrt((a * a) + (b * b));

                        // Below derived from D = VT + 1/2 * A(T^2) to determine max time a pillbug can take before colliding
                        float maxTime = Mathf.Sqrt((2 * maxDistance) / acceleration);
                        float speedMultiplier = maxTime / Mathf.Sqrt((2 * achievedDistance) / acceleration);
                        float maxSpeed = achievedSpeed * speedMultiplier;

                        float maxRecoil = (yMax * 2) * .75f;
                        deceleration = (2 * maxRecoil - 6 * maxSpeed) / 9;

                        SetRecoilDirection();
                    }

                    float distance = (achievedSpeed * timeCount) + deceleration * (timeCount * timeCount) / 2;
                    if (distance < lastDistance)
                    {
                        timeCount = 0f;
                        recoil = false;
                        deceleration = 0f;
                        totalDistance = 0f;

                        recoilDirection = -1;

                        achievedSpeed = 0f;
                        achievedTime = 0f;
                        achievedDistance = 0f;
                        lastDistance = 0f;
                        retargetDiff = 0f;
                    }
                    else
                    {
                        float eta = -achievedSpeed / deceleration;
                        transform.Rotate((Vector3.forward * (.45f + (achievedDistance * .05f)) * ((eta - timeCount) / eta))); 

                        transform.position = Vector2.MoveTowards(startingPosition, targetPosition, distance - retargetDiff);
                        lastDistance = distance;
                    }
                    timeCount += Time.deltaTime;
                }
                else
                {
                    cooldownCount += Time.deltaTime;
                    if (cooldownCount >= cooldownTime)
                    {
                        audioSource.PlayOneShot(bowling, volume);
                        SetTargetPosition();
                        cooldown = false;
                        cooldownCount = 0;

                        // speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
                        // transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                        // transform.Rotate(Vector3.forward * .5f);

                        timeCount += Time.deltaTime;
                    }
                }
            }
        }
        else if (entry.isSpawned == true)
        {
            audioSource.PlayOneShot(bowling, volume);
            startMoving = true;
            SetTargetPosition();
        }
    }

    bool CheckRecoilDirection()
    // Returns false if detects identical collision
    {                    
        startingPosition = transform.position;
        if (transform.position.y + transform.localScale.y >= yMax)
        // Top side collision
        {
            if (recoilDirection == 0)
            {
                return false;
            }
            recoilDirection = 0;
        }
        else if (transform.position.y - transform.localScale.y <= -yMax)
        // Bottom side collision
        {
            if (recoilDirection == 1)
            {
                return false;
            }
            recoilDirection = 1;
        }
        else if (transform.position.x + transform.localScale.x >= xMax)
        // Right side collision
        {
            if (recoilDirection == 2)
            {
                return false;
            }
            recoilDirection = 2;
        }
        else
        // Left side collision
        {
            if (recoilDirection == 3)
            {
                return false;
            }
            recoilDirection = 3;
        }
        return true;
    }

    void SetRecoilDirection()
    {
        slope = -slope;
        float newX;
        float newY;

        switch (recoilDirection)
        {
            case 0:
            // Down
                newY = yMax * -4;
                newX = newY / slope;
                break;
            case 1:
            // Up
                newY = yMax * 4;
                newX = newY / slope;
                break;
            case 2:
            // Left
                newX = xMax * -4;
                newY = newX * slope;
                break;
            default:
            // Right
                newX = xMax * 4;
                newY = newX * slope;
                break; 
        }

        targetPosition = new Vector2(transform.position.x + newX, transform.position.y + newY);
    }

    void SetTargetPosition()
    {
        Vector2 spiderPosition = GameObject.Find("Spider").transform.position;

        slope = (spiderPosition.y - transform.position.y) / (spiderPosition.x - transform.position.x);

        float a = xMax * 2;
        float b = yMax * 2;
        float maxDistance = Mathf.Sqrt((a * a) + (b * b));

        float xDiff = maxDistance * 2;

        if ((spiderPosition.x < transform.position.x & spiderPosition.y > transform.position.y) | (spiderPosition.x < transform.position.x & spiderPosition.y < transform.position.y))
        // Quadrant II or Quadrant III so pillbug will move right, xDiff is negative
        {
            xDiff = -xDiff;
        }

        float yDiff = xDiff * slope;
        targetPosition = new Vector2(spiderPosition.x + xDiff, spiderPosition.y + yDiff); 
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

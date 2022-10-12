using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomPatrol : MonoBehaviour
{

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    Vector2 targetPosition;

    public float minSpeed;
    public float maxSpeed;

    float speed;

    public float secondsToMaxDifficulty;

    public Entry entry;
    private bool startMoving = false;
    private int direction;
    // int direction guide: 0: down, 1: up, 2: left, 3: right
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        targetPosition = GetRandomPosition();
        SetDirection(targetPosition);
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
            if ((Vector2)transform.position != targetPosition)
            {
                speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            } else {
                targetPosition = GetRandomPosition();
                SetDirection(targetPosition);
            }
        }
        else if (entry.isSpawned == true)
        {
            startMoving = true;
            SetDirection(targetPosition);
        }
    }

    private void SetDirection(Vector2 targetPosition) {

        Vector2 currentPosition = transform.position;

        float xdiff = currentPosition.x - targetPosition.x;
        float ydiff = currentPosition.y - targetPosition.y;

        if (Mathf.Abs(xdiff) >= Mathf.Abs(ydiff)) 
            {
                if (xdiff >= 0)
                {
                    direction = 2;
                }
                else
                {
                    direction = 3;
                }
            }
            else
            {
                if (ydiff >= 0)
                {
                    direction = 0;
                }
                else
                {
                    direction = 1;
                }
            }

        anim.SetInteger("direction", direction);
    }

    Vector2 GetRandomPosition() {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector2(randomX, randomY);
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Movement;


public class FollowPatrol : MonoBehaviour
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

    private Animator anim;

    private int direction;
    // int direction guide: 0: down, 1: up, 2: left, 3: right

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        targetPosition = GameObject.Find("Spider").transform.position;
        direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startMoving)
        {
            targetPosition = GameObject.Find("Spider").transform.position;
            SetDirection(targetPosition);
            speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else if (entry.isSpawned == true) 
        {
            startMoving = true;
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

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

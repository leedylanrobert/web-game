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

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = GameObject.Find("Spider").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (startMoving)
        {
            targetPosition = GameObject.Find("Spider").transform.position;
            speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            if (entry.isOnScreen == true)
            {
                Debug.Log("Entry working");
                startMoving = true;
            }
        }

    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

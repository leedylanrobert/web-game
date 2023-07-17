using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Dash : MonoBehaviour
{
    int[] touchIds = new int[5];
    int leadingFingerId;

    GameObject[] timers = new GameObject[5];

    public int touchCount = 0;
    int dashCount = 0;

    public GameObject timer;
    public GameObject canvas;

    public bool timesUp = false;

    Vector2 targetPosition;
    public float minSpeed;
    public float maxSpeed;
    float speed;
    public float secondsToMaxDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    // Old update code is in notes
    {
        if (Input.touchCount == 1)
        {
            leadingFingerId = Input.touches[0].fingerId;
        }
        else if (Input.touchCount > 1)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId != leadingFingerId & !touchIds.Contains(touch.fingerId) & touchCount < 5 & !timesUp)
                {
                    // Place timer
                    Vector2 touchWorldPosition = Camera.main.ScreenToWorldPoint(touch.position);

                    GameObject newTimer = timer;
                    newTimer.GetComponent<SliderCountdown>().position = new Vector3(touchWorldPosition.x, touchWorldPosition.y, 2);
                    Instantiate(newTimer, new Vector3(touchWorldPosition.x, touchWorldPosition.y, 2), transform.rotation, canvas.transform);
                    // newTimer.GetComponent<SliderCountdown>().startCount = true;
                    timers[touchCount] = Instantiate(newTimer, new Vector3(touchWorldPosition.x, touchWorldPosition.y, 2), transform.rotation, canvas.transform);
                    timers[touchCount].GetComponent<SliderCountdown>().startCount = true;

                    touchCount += 1;
                }
            }
            // Debug.Log("Timers: " + timers[4].GetComponent<SliderCountdown>().startCount);
        }
        
        touchIds = Input.touches.Select(touch => touch.fingerId).ToArray();

        if (timesUp)
        {
            // Dashing
            if (GameObject.Find("Spider").GetComponent<FollowTouch>().dashing == false)
            {
                GameObject.Find("Spider").GetComponent<FollowTouch>().dashing = true;
                targetPosition = timers[0].transform.position;
            }

            if ((Vector2)transform.position == targetPosition) {
                // targetPosition = GetRandomPosition();
                // SetDirection(targetPosition);
                dashCount += 1;
                try
                {
                    if (timers[dashCount].tag == "Timer")
                    {
                        targetPosition = timers[dashCount].transform.position;
                    }
                }
                catch (Exception e)
                {
                    EndDashSequence();
                }
            }
            speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
    
    void EndDashSequence()
    {
        timesUp = false;
        GameObject.Find("Spider").GetComponent<FollowTouch>().dashing = false;

        foreach (GameObject timer in GameObject.FindGameObjectsWithTag("Timer"))
        {
            Destroy(timer);
            timers = new GameObject[5];
            touchIds = new int[5];
            touchCount = 0;
            dashCount = 0;
        }
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

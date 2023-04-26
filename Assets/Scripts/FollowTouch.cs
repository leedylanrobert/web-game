using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTouch : MonoBehaviour
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
    public bool collided = false;
    private Animator anim;
    private int direction;

    public bool deploying = true;
    // int direction guide: 0: down, 1: up, 2: left, 3: right

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        direction = 0;
    }
     
    // public void Move()
    // {
    //     Vector3 Movement = new Vector3 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    //     float speedMultiplier = 1f;
    //     // if (deploying)
    //     // {
    //     //     Debug.Log("deploying");
    //     //     speedMultiplier = .65f;
    //     // }
    //     transform.position += Movement * (speed * speedMultiplier) * Time.deltaTime;
    //     if (Movement.x == 0 & Movement.y == 0){
    //         anim.SetBool("moving", false);
    //     }else{
    //         anim.SetBool("moving", true);
    //     }
    //     if (!isPaused)
    //     {
    //         if (Movement.x < 0 & Mathf.Abs(Movement.x) > Mathf.Abs(Movement.y)){
    //             direction = 2;
    //         }else if (Movement.x > 0 & Mathf.Abs(Movement.x) > Mathf.Abs(Movement.y)){
    //             direction = 3;
    //         }else if (Movement.y < 0 & Mathf.Abs(Movement.y) > Mathf.Abs(Movement.x)){
    //             direction = 0;
    //         }else if (Movement.y > 0 & Mathf.Abs(Movement.y) > Mathf.Abs(Movement.x)){
    //             direction = 1;
    //         }
    //     }
    //     anim.SetInteger("direction", direction);
    // }

    // Update is called once per frame
    void Update()
    {
        // Touch controls
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            targetPosition = touchPosition;
        }

        Vector2 currentPosition = transform.position;
        if (targetPosition != null & targetPosition != currentPosition) 
        {
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
            anim.SetBool("moving", true);
            anim.SetInteger("direction", direction);
        }
        else
        {
            anim.SetBool("moving", false);
        }

        speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
        // Comment out below line to enable keyboard control
        // transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Keyboard controls
        Vector3 Movement = new Vector3 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        float speedMultiplier = 1f;
        transform.position += Movement * (speed * speedMultiplier) * Time.deltaTime;
        if (Movement.x == 0 & Movement.y == 0){
            anim.SetBool("moving", false);
        }else{
            anim.SetBool("moving", true);
        }
            if (Movement.x < 0 & Mathf.Abs(Movement.x) > Mathf.Abs(Movement.y)){
                direction = 2;
            }else if (Movement.x > 0 & Mathf.Abs(Movement.x) > Mathf.Abs(Movement.y)){
                direction = 3;
            }else if (Movement.y < 0 & Mathf.Abs(Movement.y) > Mathf.Abs(Movement.x)){
                direction = 0;
            }else if (Movement.y > 0 & Mathf.Abs(Movement.y) > Mathf.Abs(Movement.x)){
                direction = 1;
            }
        anim.SetInteger("direction", direction);
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

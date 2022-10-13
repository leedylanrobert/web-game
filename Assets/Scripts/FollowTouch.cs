using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTouch : MonoBehaviour
{
    private int speed = 12;

    private Animator anim;
    private int direction;

    public bool isPaused = false;

    public bool deploying = false;
    // int direction guide: 0: down, 1: up, 2: left, 3: right

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        direction = 0;
    }
     
    public void Move()
    {
        Vector3 Movement = new Vector3 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        float speedMultiplier = 1f;
        if (deploying)
        {
            speedMultiplier = .65f;
        }
        transform.position += Movement * (speed * speedMultiplier) * Time.deltaTime;
        if (Movement.x == 0 & Movement.y == 0){
            anim.SetBool("moving", false);
        }else{
            anim.SetBool("moving", true);
        }
        if (!isPaused)
        {
            if (Movement.x < 0 & Mathf.Abs(Movement.x) > Mathf.Abs(Movement.y)){
                direction = 2;
            }else if (Movement.x > 0 & Mathf.Abs(Movement.x) > Mathf.Abs(Movement.y)){
                direction = 3;
            }else if (Movement.y < 0 & Mathf.Abs(Movement.y) > Mathf.Abs(Movement.x)){
                direction = 0;
            }else if (Movement.y > 0 & Mathf.Abs(Movement.y) > Mathf.Abs(Movement.x)){
                direction = 1;
            }
        }
        anim.SetInteger("direction", direction);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}

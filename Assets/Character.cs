using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public class Character : MonoBehaviour {

    public Walkable walkable;
    public Entry entry;
    private int direction;
    private Animator anim;

    private bool startMoving = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        direction = 0;
    }

    private void Update() 
    {
        if (startMoving)
        {
            Transform target = GameObject.Find("Spider").transform;
            SetDirection(target.position);
            var directionTowardsTarget = (target.position - this.transform.position).normalized;
            walkable.MoveTo(directionTowardsTarget);
        }
        else if (entry.isSpawned == true)
        {
            startMoving = true;
            Transform target = GameObject.Find("Spider").transform;
            SetDirection(target.position);
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
}

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
    public GameObject restartPanel;

    public AudioClip loss;
    float volume = 1;
    public bool collided = false;

    private Animator anim;
    private int direction;
    // int direction guide: 0: down, 1: up, 2: left, 3: right

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentPosition = transform.position;
        if (targetPosition != null & targetPosition != currentPosition) 
        {
            float xdiff = currentPosition.x - targetPosition.x;
            float ydiff = currentPosition.y - targetPosition.y;
            
            if (Mathf.Abs(xdiff) >= Mathf.Abs(ydiff)) 
            {
                if (xdiff >= 1)
                {
                    Debug.Log("Left");
                    direction = 2;
                }
                else
                {
                    Debug.Log("Right");
                    direction = 3;
                }
            }
            else
            {
                if (ydiff >= 1)
                {
                    Debug.Log("Down");
                    direction = 0;
                }
                else
                {
                    Debug.Log("Up");
                    direction = 1;
                }
            }
            Debug.Log("Moving");
            anim.SetBool("moving", true);
            anim.SetInteger("direction", direction);
        }
        else
        {
            Debug.Log("Idle");
            anim.SetBool("moving", false);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            targetPosition = touchPosition;
        }

        speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    Vector2 GetRandomPosition() {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector2(randomX, randomY);
    }

      private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!collided)
            {
                AudioSource.PlayClipAtPoint(loss, transform.position, volume);
                collided = true;
            }

            Debug.Log("collided");
            restartPanel.SetActive(true);
        }
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

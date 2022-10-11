using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Entry : MonoBehaviour
{
    public bool isSpawned = false;
    private Vector2 position;
    private Vector2 targetPosition;

    public TMP_Text countdown;
    public SpriteRenderer enemy;

    private float timeElapsed = 0f;
    private TMP_Text selfText;

    public AudioClip tick;
    public AudioClip spawn;
    float volume = 1;
    private int playedTimes = 1;

    private float xMax;
    private float yMax;

    // Start is called before the first frame update
    void Start()
    {

        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 topRightWorld = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        xMax = topRightWorld.x;
        yMax = topRightWorld.y;

        RectTransform canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        //float xMax = canvas.rect.width / 2;

        // switch (side)
        // {
        //     case 0:
        //         // Up
        //         newX = Random.Range(-xMax, xMax);
        //         newY = 5.4f;
        //         position = new Vector2(newX,newY);
        //         // transform.position = position;
        //         countdownPosition = new Vector2(newX, newY - .4f);
        //         break;
        //     case 1:
        //         // Right
        //         newX = (160f / 18f) + .4f;
        //         newY = Random.Range(-4.6f, 4.6f);
        //         position = new Vector2(newX,newY);
        //         // transform.position = position;
        //         countdownPosition = new Vector2(newX - 1.0f, newY);
        //         break;
        //     case 2:
        //         // Down
        //         newX = Random.Range(-xMax, xMax);
        //         newY = -5.4f;
        //         position = new Vector2(newX,newY);
        //         // transform.position = position;
        //         countdownPosition = new Vector2(newX, newY + 1.2f);
        //         break;
        //     case 3: 
        //         // Left
        //         newX = (-160f / 18f) - .4f;
        //         newY = Random.Range(-4.6f, 4.6f);
        //         position = new Vector2(newX,newY);
        //         // transform.position = position;
        //         countdownPosition = new Vector2(newX + .6f, newY);
        //         break;
        //     default:
        //         countdownPosition = new Vector2(newX + .8f, newY);
        //         break;
        // }
        Vector3 position = transform.position;
        Vector3 countdownPosition;

        Debug.Log("Entry Position: " + position);

        if (position.y > yMax) 
        {
            // Up
            countdownPosition = new Vector3(position.x, position.y - .6f, 1);
        }
        else if (position.x > xMax)
        {
            // Right
            countdownPosition = new Vector3(position.x - 1.2f, 1);
        }
        else if (position.y < -yMax)
        {
            // Down
            countdownPosition = new Vector3(position.x, position.y + 1.3f);
        }
        else
        {
            // Left
            countdownPosition = new Vector3(position.x + .8f, 1);
        }

        Transform parent = GameObject.Find("Canvas").transform;
        Vector3 countdownVector3 = position;

        if (enemy.color.r == 1.00) {
            countdown.color = new Color(1.0f, 0.902f, 0.251f, 1.0f);
        }
        else if (enemy.color.r == 0) {
            countdown.color = new Color(0.058f, 0.538f, 0.146f, 1.0f);
        }
        else {
            countdown.color = enemy.color;
        }

        countdown.text = "3";
        selfText = Instantiate(countdown, countdownPosition, transform.rotation, parent);
        AudioSource.PlayClipAtPoint(tick, transform.position, volume);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed > 4f)
        {
            Destroy(selfText);
        }
        if (timeElapsed > 3f)
        {
            isSpawned = true;
            selfText.text = "!";
            countdown.color = Color.white;
            if (playedTimes < 4)
            {
                AudioSource.PlayClipAtPoint(spawn, transform.position, volume);
                playedTimes++;
            }
        }
        else if (timeElapsed > 2f)
        {
            selfText.text = "1";
            countdown.color = enemy.color;
            if (playedTimes < 3)
            {
                AudioSource.PlayClipAtPoint(tick, transform.position, volume);
                playedTimes++;
            }
        }
        else if (timeElapsed > 1f)
        {
            selfText.text = "2";
            countdown.color = Color.white;
            if (playedTimes < 2)
            {
                AudioSource.PlayClipAtPoint(tick, transform.position, volume);
                playedTimes++;
            }
        }
        timeElapsed += Time.deltaTime;
    }
}


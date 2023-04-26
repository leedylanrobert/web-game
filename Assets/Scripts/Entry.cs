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
    public Font font;

    private float timeElapsed = 0f;
    private TMP_Text selfText;

    public AudioClip tick;
    public AudioClip spawn;
    float volume = 0.75f;
    private int playedTimes = 1;

    private float xMax;
    private float yMax;
    private float xMin;
    private float yMin;

    public float secondsToMaxDifficulty;
    private float tickInterval = 4f;
    private float baseTickInterval = 4f;

    private Vector3 countdownPosition;

    private Vector2 soundPosition = new Vector2(0f, 0f);

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 topRightWorld = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector3 bottomLeftWorld = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        xMax = topRightWorld.x;
        yMax = topRightWorld.y;
        xMin = bottomLeftWorld.x;
        yMin = bottomLeftWorld.y;

        RectTransform canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();

        Vector3 position = transform.position;

        if (position.y > yMax) 
        {
            // Up
            countdownPosition = new Vector3(position.x, position.y - 1.3f, 1);
        }
        else if (position.x > xMax)
        {
            // Right
            countdownPosition = new Vector3(position.x - 1.3f, position.y);
        }
        else if (position.y < -yMax)
        {
            // Down
            countdownPosition = new Vector3(position.x, position.y + 1.3f);
        }
        else
        {
            // Left
            countdownPosition = new Vector3(position.x + 1.3f, position.y);
        }

        Transform parent = GameObject.Find("Canvas").transform;
        Vector3 countdownVector3 = position;

        switch (gameObject.name)
        {
            case "Bee(Clone)":
                countdown.color = Color.yellow;
                break;
            case "Dragonfly(Clone)":
                countdown.color = Color.blue;
                break;
            case "Pillbug(Clone)":
                countdown.color = Color.grey;
                break;
            case "Fly(Clone)":
                countdown.color = Color.green;
                break;
            case "Scorpion(Clone)":
                countdown.color = new Color(140f/255f, 65f / 255f, 0);
                break;
            default:
                countdown.color = Color.red;
                break;
        }
        // if (enemy.color.r == 1.00) {
        //     countdown.color = new Color(1.0f, 0.902f, 0.251f, 1.0f);
        // }
        // else if (enemy.color.r == 0) {
        //     countdown.color = new Color(0.058f, 0.538f, 0.146f, 1.0f);
        // }
        // else {
        //     countdown.color = enemy.color;
        // }

        countdown.text = "3";
        selfText = Instantiate(countdown, countdownPosition, transform.rotation, parent);
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(tick, volume);
    }

    void Update()
    {
        CountDown();

        tickInterval = baseTickInterval - ((baseTickInterval * (2f / 3f)) * GetDifficultyPercent());
        timeElapsed += Time.deltaTime;
    }

    void CountDown()
    {
        if (timeElapsed > tickInterval)
        {
            Destroy(selfText);
        }
        if (timeElapsed > (tickInterval * (3f / 4f)))
        {
            isSpawned = true;
            selfText.text = "!";
            if (playedTimes < 4)
            {
                audioSource.PlayOneShot(spawn, volume);
                playedTimes++;
            }
        }
        else if (timeElapsed > (tickInterval / 2f))
        {
            selfText.text = "1";
            if (playedTimes < 3)
            {
                audioSource.PlayOneShot(tick, volume);
                playedTimes++;
            }
        }
        else if (timeElapsed > (tickInterval / 4f))
        {
            selfText.text = "2";
            if (playedTimes < 2)
            {
                audioSource.PlayOneShot(tick, volume);
                playedTimes++;
            }
        }
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}


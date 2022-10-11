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
    float volume = 0.75f;
    private int playedTimes = 1;

    private float xMax;
    private float yMax;

    public float secondsToMaxDifficulty;
    private float tickInterval = 4f;
    private float baseTickInterval = 4f;

    private Vector2 soundPosition = new Vector2(0f, 0f);

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 topRightWorld = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        xMax = topRightWorld.x;
        yMax = topRightWorld.y;

        RectTransform canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();

        Vector3 position = transform.position;
        Vector3 countdownPosition;

        if (position.y > yMax) 
        {
            // Up
            countdownPosition = new Vector3(position.x, position.y - 0.8f, 1);
        }
        else if (position.x > xMax)
        {
            // Right
            countdownPosition = new Vector3(position.x - 1.4f, 1);
        }
        else if (position.y < -yMax)
        {
            // Down
            countdownPosition = new Vector3(position.x, position.y + 1.5f);
        }
        else
        {
            // Left
            countdownPosition = new Vector3(position.x + 1.0f, position.y);
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
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(tick, volume);
    }

    // Update is called once per frame
    void Update()
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
        tickInterval = baseTickInterval - ((baseTickInterval * (2f / 3f)) * GetDifficultyPercent());
        timeElapsed += Time.deltaTime;
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}


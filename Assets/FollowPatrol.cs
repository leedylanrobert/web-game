using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject restartPanel;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = GameObject.Find("Spider").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
            targetPosition = GameObject.Find("Spider").transform.position;
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
        if (collision.gameObject.name == "Spider")
        {
            Debug.Log("collided");
            restartPanel.SetActive(true);
        }
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

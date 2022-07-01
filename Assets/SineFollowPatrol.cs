using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SineFollowPatrol : MonoBehaviour
{

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    Vector2 targetPosition;
    Vector2 basePosition;

    public float minSpeed;
    public float maxSpeed;

    float speed;

    public float secondsToMaxDifficulty;

    public float counter = 0f;
    public float counterIncrement = 0.00001f;
    public float waveIntensity = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = GameObject.Find("Spider").transform.position;
        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
            targetPosition = GameObject.Find("Spider").transform.position;
            speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
            basePosition = Vector2.MoveTowards(basePosition, targetPosition, speed * Time.deltaTime);

            // Get slope of perpendicular line
            float slope = -1f / ((targetPosition.y - basePosition.y) / (targetPosition.x - basePosition.x));
            float yIntercept = basePosition.y - (slope * basePosition.x);
            float perpX = basePosition.x - waveIntensity;
            Vector2 perpPoint = new Vector2(perpX, (slope * perpX) + yIntercept);
            transform.position = Vector2.MoveTowards(basePosition, perpPoint, Mathf.Sin(counter) * waveIntensity);

            counter += counterIncrement;

    }

    Vector2 GetRandomPosition() {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector2(randomX, randomY);
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

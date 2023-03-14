using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnEnemy : MonoBehaviour
{
    public bool keepSpawning = true;

    public GameObject ant;
    public GameObject bee;
    public GameObject dragonfly;
    private GameObject[] enemies;

    private float xMax;
    private float yMax;

    public float deltaTimeCounter = 6.0f;
    private System.Random random = new System.Random();

    public float secondsToMaxDifficulty;
    public float deltaTimeInterval = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new GameObject[]{ant, bee, dragonfly};

        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 topRightWorld = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        xMax = topRightWorld.x;
        yMax = topRightWorld.y;
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 topRightWorld = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        xMax = topRightWorld.x;
        yMax = topRightWorld.y;

        Spawn();
        deltaTimeInterval = 6.0f - (4.0f * GetDifficultyPercent());
    }

    void Spawn()
    {
        if (keepSpawning) {

            deltaTimeCounter += Time.deltaTime;

            if (deltaTimeCounter >= deltaTimeInterval)
            {
                // Get random enemy
                int enemyIndex = random.Next(3);
                GameObject randomEnemy = enemies[enemyIndex];

                // Get random spawn point
                deltaTimeCounter = 0f;
                Instantiate(randomEnemy, RandomPosition(), transform.rotation);
            }
        }
    }

    Vector3 RandomPosition() 
    {
        int side = Random.Range(0,4);
        // int side guide: 0: up, 1: right, 2: down, 3: left

        float newX = 0f;
        float newY = 0f;

        Vector3 position;
        RectTransform canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();

        switch (side)
        {
            case 0:
                // Up
                float[] xVals = new float[]{Random.Range(-xMax + (xMax / 2.8f), -xMax / 3f), Random.Range(xMax / 3f, xMax - 0.6f)};
                // float[] xVals = new float[]{-xMax + (xMax / 2.8f), xMax / 3f};
                // newX = Random.Range(-xMax + 0.6f, xMax - 0.6f);
                newX = xVals[Random.Range(0, 2)];

                newY = yMax + 0.8f;
                position = new Vector2(newX,newY);
                break;
            case 1:
                // Right
                newX = xMax + 0.8f;
                newY = Random.Range(-yMax + 1.2f, yMax - 1.2f);
                position = new Vector2(newX, newY);
                break;
            case 2:
                // Down
                newX = Random.Range(-xMax + 1.2f, xMax - 1.2f);
                newY = -yMax - 0.8f;
                position = new Vector2(newX, newY);
                break;
            case 3: 
                // Left
                newX = -xMax - 0.8f;
                newY = Random.Range(-yMax + 1.2f, yMax - 1.2f);
                position = new Vector2(newX, newY);
                break;
            default:
                position = new Vector2(newX, newY);
                break;
        }
        return position;
    }


    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

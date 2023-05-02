using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Orbit : MonoBehaviour
{
    private GameObject target;
    private float lastX;
    private float lastY;
    private System.Random random = new System.Random();
    private bool onTarget = false;

    public float minSpeed;
    public float maxSpeed;
    public float secondsToMaxDifficulty;
    float speed;
    float radius = 2f;

    float angle;
    float rotationFactor = 0.0035f;

    private Vector2 targetPosition;

    private bool startMoving = false;
    public Entry entry;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            onTarget = false;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] validEnemies = enemies.Where(enemy => enemy.name != "Fly(Clone)").ToArray();

            if (validEnemies.Length > 0)
            {
                target = validEnemies[random.Next(validEnemies.Length)];
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Spider");
            }
        }
        if (startMoving)
        {
            float flyX = transform.position.x;
            float flyY = transform.position.y;

            float centerX = target.transform.position.x;
            float centerY = target.transform.position.y;

            float xDiff = centerX - flyX;
            float yDiff = centerY - flyY;

            if (!onTarget)
            {
                if (Mathf.Sqrt((xDiff * xDiff) + (yDiff * yDiff)) <= radius)
                {
                    onTarget = true;

                    angle = Mathf.Atan2(flyY - centerY, flyX - centerX) - Mathf.Atan2(1, 0);

                    float newX = radius * Mathf.Cos(angle);
                    float newY = radius * Mathf.Sin(angle);

                    transform.position = new Vector2(target.transform.position.x - newX, newY + target.transform.position.y);
                    // -3.093354
                }
                else
                {

                    speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
                    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                }
            }
            else
            {
                angle += rotationFactor;

                float newX = radius * Mathf.Sin(angle);
                float newY = radius * Mathf.Cos(angle);

                transform.position = new Vector2(target.transform.position.x - newX, newY + target.transform.position.y);
            }
        }
        else if (entry.isSpawned == true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] validEnemies = enemies.Where(enemy => enemy.name != "Fly(Clone)" & enemy.GetComponent<EnemyProps>().isSpawned == true).ToArray();

            if (validEnemies.Length > 0)
            {
                target = validEnemies[random.Next(validEnemies.Length)];
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Spider");
            }
            
            startMoving = true;
        }
    }

    float GetDifficultyPercent() {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}

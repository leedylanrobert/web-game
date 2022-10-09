using System;
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
    // public GameObject beetle;
    private GameObject[] enemies;

    public float interval = 250;
    private float counter = 250;
    private System.Random random = new System.Random();
    private Vector3[] spawnPoints = new Vector3[]{new Vector3(-7.5f, 3.6f, 0), new Vector3(-7.5f, -3.6f, 0), new Vector3(7.5f, 3.6f, 0), new Vector3(7.5f, -3.6f, 0)};

    // Start is called before the first frame update
    void Start()
    {
      enemies = new GameObject[]{ant, bee, dragonfly};
    }

    // Update is called once per frame
    void Update()
    {
        if (keepSpawning) {
            counter += 1;
            if(counter >= interval)
            {
                // Get random enemy
                int enemyIndex = random.Next(3);
                GameObject randomEnemy = enemies[enemyIndex];

                // Get random spawn point
                int randomIndex = random.Next(3);
                Vector3 randomPoint = spawnPoints[randomIndex];
                counter = 0;
                Instantiate(randomEnemy, randomPoint, transform.rotation);
            }
        }
    }
}

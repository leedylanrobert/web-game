using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public float interval = 500;
    private float counter = 0;
    private System.Random random = new System.Random();
    private Vector3[] spawnPoints = new Vector3[]{new Vector3(-7.5f, 3.6f, 0), new Vector3(-7.5f, -3.6f, 0), new Vector3(7.5f, 3.6f, 0), new Vector3(7.5f, -3.6f, 0)};

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        counter += 1;
        if(counter >= interval)
        {
            // Get random spawn point
            int randomIndex = random.Next(4);
            Vector3 randomPoint = spawnPoints[randomIndex];
            counter = 0;
            Instantiate(enemy, randomPoint, transform.rotation);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    public bool isSpawned = false;
    private Vector2 position;
    private Vector2 targetPosition;
    public float entrySpeed;

    // Start is called before the first frame update
    void Start()
    {
        int side = Random.Range(0,4);
        // int side guide: 0: up, 1: right, 2: down, 3: left

        float newX = 0f;
        float newY = 0f;
        float xMax = (160f / 18f) -.4f;

        switch (side)
        {
            case 0:
                Debug.Log("Up");
                newX = Random.Range(-xMax, xMax);
                newY = 5.4f;
                position = new Vector2(newX,newY);
                transform.position = position;
                break;
            case 1:
                Debug.Log("Right");
                newX = (160f / 18f) + .4f;
                newY = Random.Range(-4.6f, 4.6f);
                position = new Vector2(newX,newY);
                transform.position = position;
                break;
            case 2:
                Debug.Log("Down");
                newX = Random.Range(-xMax, xMax);
                newY = -5.4f;
                position = new Vector2(newX,newY);
                transform.position = position;
                break;
            case 3:
                Debug.Log("Left");
                newX = (-160f / 18f) - .4f;
                newY = Random.Range(-4.6f, 4.6f);
                position = new Vector2(newX,newY);
                transform.position = position;
                break;
            default:
                Debug.Log("Default");
                break;
        }
        isSpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}


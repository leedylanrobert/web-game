using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Entry : MonoBehaviour
{
    public bool isSpawned = false;
    private Vector2 position;
    private Vector2 targetPosition;

    public GameObject countdown;
    public TMP_Text test;
    public SpriteRenderer enemy;

    // Start is called before the first frame update
    void Start()
    {
        int side = Random.Range(0,4);
        // int side guide: 0: up, 1: right, 2: down, 3: left

        float newX = 0f;
        float newY = 0f;
        float xMax = (160f / 18f) -.4f;

        Vector2 countdownPosition;

        switch (side)
        {
            case 0:
                // Up
                newX = Random.Range(-xMax, xMax);
                newY = 5.4f;
                position = new Vector2(newX,newY);
                transform.position = position;
                countdownPosition = new Vector2(newX, newY - .4f);
                break;
            case 1:
                // Right
                newX = (160f / 18f) + .4f;
                newY = Random.Range(-4.6f, 4.6f);
                position = new Vector2(newX,newY);
                transform.position = position;
                countdownPosition = new Vector2(newX - 1.0f, newY);
                break;
            case 2:
                // Down
                newX = Random.Range(-xMax, xMax);
                newY = -5.4f;
                position = new Vector2(newX,newY);
                transform.position = position;
                countdownPosition = new Vector2(newX, newY + 1.2f);
                break;
            case 3: 
                // Left
                newX = (-160f / 18f) - .4f;
                newY = Random.Range(-4.6f, 4.6f);
                position = new Vector2(newX,newY);
                transform.position = position;
                countdownPosition = new Vector2(newX + .6f, newY);
                break;
            default:
                Debug.Log("Default");
                countdownPosition = new Vector2(newX + .8f, newY);
                break;
        }
        Transform parent = GameObject.Find("Canvas").transform;
        Vector3 countdownVector3 = countdownPosition;
        test.color = enemy.color;
        Instantiate(test, countdownVector3, transform.rotation, parent);
        isSpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}


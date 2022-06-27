using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy; //assign the enemy in editor.
    public float interval = 100;
    private float counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter +=1;

        if(counter >= interval)
        {
            counter = 0;
            Instantiate(enemy, transform.position, transform.rotation);
        }
    }
}

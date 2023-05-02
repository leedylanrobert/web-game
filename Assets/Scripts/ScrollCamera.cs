using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCamera : MonoBehaviour
{
    GameObject spider;

    // Start is called before the first frame update
    void Start()
    {
        spider = GameObject.Find("Spider");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(spider.transform.position.x, spider.transform.position.y, -10);
    }
}

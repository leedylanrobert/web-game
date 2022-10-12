using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInside : MonoBehaviour
{
    float xMax;
    float yMax;
    
    // Start is called before the first frame update
    void Start()
    {
      Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    Vector3 topRightWorld = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

    xMax = topRightWorld.x;
    yMax = topRightWorld.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xMax, xMax), Mathf.Clamp(transform.position.y, -yMax, yMax), transform.position.z);
    }
}

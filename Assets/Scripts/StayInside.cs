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
        Debug.Log("Position: " + transform.position);

        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 topRightWorld = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        xMax = topRightWorld.x;
        yMax = topRightWorld.y;

        Debug.Log("Xmax: " + xMax);
        Debug.Log("Ymax: " + yMax);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Position: " + transform.position);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xMax, xMax), Mathf.Clamp(transform.position.y, -yMax, yMax), transform.position.z);
    }
}

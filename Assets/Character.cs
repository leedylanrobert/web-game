using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public class Character : MonoBehaviour {

    public Walkable walkable;
    public Entry entry;

    private bool startMoving = false;

    private void Update() 
    {
        if (startMoving)
        {
            Transform target = GameObject.Find("Spider").transform;
            var directionTowardsTarget = (target.position - this.transform.position).normalized;
            walkable.MoveTo(directionTowardsTarget);
        }
        else if (entry.isSpawned == true)
        {
            startMoving = true;
        }

    }
}

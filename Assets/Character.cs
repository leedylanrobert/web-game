using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public class Character : MonoBehaviour {

    public Walkable walkable;
    public Entry Entry;

    private void Update() {
        
        Transform target = GameObject.Find("Spider").transform;
        var directionTowardsTarget = (target.position - this.transform.position).normalized;
        walkable.MoveTo(directionTowardsTarget);
    }
}

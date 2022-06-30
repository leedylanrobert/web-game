using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public class Character : MonoBehaviour {

    public Transform target;
    public Walkable walkable;

    private void Update() {
        Debug.Log("Character");
        var directionTowardsTarget = (target.position - this.transform.position).normalized;
        Debug.Log("Dir to Target: " + directionTowardsTarget);
        walkable.MoveTo(directionTowardsTarget);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Walkable : MonoBehaviour {

        private const float ForcePower = 10f;

        public new Rigidbody2D rigidbody;

        public float speed = 2f;
        public float force = 2f;

        private Vector2 direction;

        public void MoveTo (Vector2 direction) {
            this.direction = direction;
        }

        public void Stop() {
            MoveTo(Vector2.zero);
        }

        private void FixedUpdate() {
            Debug.Log("fixed update");
            var desiredVelocity = direction * speed;
            Debug.Log("Desire velocity: " + desiredVelocity);
            var deltaVelocity = desiredVelocity - rigidbody.velocity;
            Debug.Log("Delta velocity: " + deltaVelocity);
            Vector3 moveForce = deltaVelocity * (force * ForcePower * Time.fixedDeltaTime);
            Debug.Log("move force: " + moveForce);
            rigidbody.AddForce(moveForce);
        }
    }
}


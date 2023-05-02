using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Walkable : MonoBehaviour {

        private const float ForcePower = 10f;

        public new Rigidbody2D rigidbody;

        public float minSpeed = 2f;
        public float maxSpeed = 4f;

        public float secondsToMaxDifficulty;

        public float force = 2f;

        private Vector2 direction;

        public void MoveTo (Vector2 direction) {
            this.direction = direction;
        }

        public void Stop() {
            MoveTo(Vector2.zero);
        }

        private void FixedUpdate() 
        {
            var desiredVelocity = direction * Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
            var deltaVelocity = desiredVelocity - rigidbody.velocity;
            Vector3 moveForce = (deltaVelocity * (force * ForcePower * Time.deltaTime)) * 150f;
            rigidbody.AddForce(moveForce);
        }

        float GetDifficultyPercent() {
            return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
        }
    }

}


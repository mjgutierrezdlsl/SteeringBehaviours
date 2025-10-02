using UnityEngine;

namespace DLSL.SteeringBehaviours.Boids
{
    public class Boid : MonoBehaviour
    {
        [field: SerializeField] public float MaxSpeed { get; private set; } = 5f;
        [SerializeField] private int _maxRaycasts = 10;
        [SerializeField] private float _viewAngle = 360f;
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// The "stick together" rule.
        /// Each boid tries to move towards the average position of its neigbhors
        /// </summary>
        public Vector3 Cohesion(Boid[] neighbors)
        {
            Vector3 centerOfMass = Vector3.zero;
            int count = 0;

            foreach (var neighbor in neighbors)
            {
                if (neighbor != this)
                {
                    centerOfMass += neighbor.transform.position;
                    count++;
                }
            }

            if (count > 0)
            {
                centerOfMass /= count;
                return (centerOfMass - transform.position).normalized;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// This prevents the boids from colliding.
        /// Each boid tries to move away from its close neighbors.
        /// The most efficient implementation scales the avoidance force inversely with distance.
        /// </summary>
        public Vector3 Separation(Boid[] neighbors, float separationRadius)
        {
            Vector3 moveAway = Vector3.zero;
            int count = 0;

            foreach (var neighbor in neighbors)
            {
                if (neighbor != this && Vector3.Distance(transform.position, neighbor.transform.position) < separationRadius)
                {
                    Vector3 difference = transform.position - neighbor.transform.position;
                    moveAway += difference.normalized / difference.magnitude;
                    count++;
                }
            }

            if (count > 0)
            {
                moveAway /= count;
            }

            return moveAway.normalized;
        }

        /// <summary>
        /// This makes the boids move in the same direction.
        /// Each boid tries to match the average velocity of its neighbors.
        /// </summary>
        public Vector3 Alignment(Boid[] neighbors)
        {
            Vector3 averageVelocity = Vector3.zero;
            int count = 0;

            foreach (var neighbor in neighbors)
            {
                if (neighbor != this)
                {
                    averageVelocity += neighbor.Velocity;
                    count++;
                }
            }

            if (count > 0)
            {
                averageVelocity /= count;
                return averageVelocity.normalized;
            }

            return Vector3.zero;
        }

        public Vector3 Avoidance(LayerMask obstacleLayer, float viewRange)
        {
            Vector3 direction = Vector3.zero;
            var delta = _viewAngle / _maxRaycasts;
            for (int i = 0; i < _maxRaycasts; i++)
            {
                var dir = Quaternion.Euler(0, 0, i * delta) * transform.right;
                var hit = Physics2D.Raycast(transform.position, dir, viewRange, obstacleLayer);
                Debug.DrawRay(transform.position, dir * viewRange, hit ? Color.red : Color.cyan);
                direction += (Vector3)hit.normal;
            }
            return direction;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DLSL.SteeringBehaviours.Boids
{
    public class FlockManager : MonoBehaviour
    {
        [SerializeField] private Boid _boidPrefab;
        [SerializeField] private int _flockSize = 50;
        [SerializeField] private float _spawnRadius = 10f;
        [SerializeField] private float _cohesionWeight = 1f;
        [SerializeField] private float _separationWeight = 1f;
        [SerializeField] private float _alignmentWeight = 1f;
        [SerializeField] private float _separationRadius = 2f;

        private List<Boid> _boids = new();

        private void Start()
        {
            for (int i = 0; i < _flockSize; i++)
            {
                var boid = Instantiate(_boidPrefab, Random.insideUnitCircle * _spawnRadius, Quaternion.identity, transform);
                boid.Velocity = Random.insideUnitCircle.normalized * boid.MaxSpeed;
                _boids.Add(boid);
            }
        }
        private void Update()
        {
            foreach (var boid in _boids)
            {
                var neighbors = FindNeighbors(boid, 5f);
                var cohesion = boid.Cohesion(neighbors) * _cohesionWeight;
                var separation = boid.Separation(neighbors, _separationRadius) * _separationWeight;
                var alignment = boid.Alignment(neighbors) * _alignmentWeight;

                boid.Velocity += cohesion + separation + alignment;
                boid.Velocity = Vector3.ClampMagnitude(boid.Velocity, boid.MaxSpeed);
                boid.transform.position += boid.Velocity * Time.deltaTime;
                boid.transform.up = boid.Velocity;
            }
        }
        private Boid[] FindNeighbors(Boid boid, float radius)
        {
            var neighbors = new List<Boid>();
            foreach (var otherBoid in _boids)
            {
                if (otherBoid != boid && Vector3.Distance(boid.transform.position, otherBoid.transform.position) < radius)
                {
                    neighbors.Add(otherBoid);
                }
            }
            return neighbors.ToArray();
        }
    }
}

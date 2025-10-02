using UnityEngine;

namespace DLSL.SteeringBehaviours.Boids
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _obstaclePrefab;
        [SerializeField] private float _spawnRadius = 15f;
        [SerializeField] private int _count = 10;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }
        private void Start()
        {
            for (int i = 0; i < _count; i++)
            {

                // Instantiate(_obstaclePrefab, Random.insideUnitCircle * _spawnRadius, Quaternion.identity, transform);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Instantiate(_obstaclePrefab, mousePos, Quaternion.identity, transform);
            }
        }
    }
}

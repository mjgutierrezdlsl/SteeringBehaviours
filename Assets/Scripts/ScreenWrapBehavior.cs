using UnityEngine;

namespace DLSL.SteeringBehaviours
{
    public class ScreenWrapBehavior : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var screenPosition = _camera.WorldToScreenPoint(transform.position);

            var rightSideOfScreenInWorld = _camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
            var leftSideOfScreenInWorld = _camera.ScreenToWorldPoint(Vector2.zero).x;

            var topOfScreenInWorld = _camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
            var bottomOfScreenInWorld = _camera.ScreenToWorldPoint(Vector2.zero).y;

            if (screenPosition.x <= 0)
            {
                transform.position = new(rightSideOfScreenInWorld, transform.position.y);
            }
            else if (screenPosition.x >= Screen.width)
            {
                transform.position = new(leftSideOfScreenInWorld, transform.position.y);
            }
            else if (screenPosition.y >= Screen.height)
            {
                transform.position = new(transform.position.x, bottomOfScreenInWorld);
            }
            else if (screenPosition.y <= 0)
            {
                transform.position = new(transform.position.x, topOfScreenInWorld);
            }
        }
    }
}

using UnityEngine;

namespace tbs
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private Transform bulletHitVfxPrefab;

        private Vector3 _targetPosition;

        public void Setup(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        private void Update()
        {
            Vector3 moveDir = (_targetPosition - transform.position).normalized;

            float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

            float moveSpeed = 100f;
            transform.position += moveDir * (moveSpeed * Time.deltaTime);

            float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

            // avoid overshooting
            if (distanceBeforeMoving < distanceAfterMoving)
            {
                transform.position = _targetPosition;

                // unparent trail renderer, so it is not destroyed with parent game object
                trailRenderer.transform.parent = null;

                Destroy(gameObject);
                Instantiate(bulletHitVfxPrefab, _targetPosition, Quaternion.identity);
            }
        }

    }
}
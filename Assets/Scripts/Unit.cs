using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _stoppingDistance = 0.5f;
    [SerializeField] private float _moveSpeed = 4f;

    private Vector3 _targetPosition;

    private void Update()
    {
        if (Vector3.Distance(_targetPosition, transform.position) > _stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * (Time.deltaTime * _moveSpeed);
    
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    public void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
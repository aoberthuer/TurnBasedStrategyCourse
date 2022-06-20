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
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(new Vector3(4, 0, 4));
        }
    }

    public void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
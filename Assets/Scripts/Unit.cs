using System;
using TreeEditor;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _stoppingDistance = 0.5f;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotateSpeed = 10f;

    private Vector3 _targetPosition;

    private Animator _unitAnimator;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _unitAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Vector3.Distance(_targetPosition, transform.position) > _stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * (Time.deltaTime * _moveSpeed);
            
            // transform.forward = moveDirection; without interpolation
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
            
            _unitAnimator.SetBool(IsWalking, true);
        }
        else
        {
            _unitAnimator.SetBool(IsWalking, false);
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
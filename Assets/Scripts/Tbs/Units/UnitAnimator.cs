using System;
using tbs.actions;
using UnityEngine;

namespace tbs.units
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        private void Awake()
        {
            if (TryGetComponent(out MoveAction moveAction))
            {
                moveAction.OnStartMoving += MoveAction_OnStartMoving;
                moveAction.OnStopMoving += MoveAction_OnStopMoving;
            }

            if (TryGetComponent(out ShootAction shootAction))
            {
                shootAction.OnShoot += ShootAction_OnShoot;
            }
        }

        private void MoveAction_OnStartMoving()
        {
            animator.SetBool(IsWalking, true);
        }

        private void MoveAction_OnStopMoving()
        {
            animator.SetBool(IsWalking, false);
        }

        private void ShootAction_OnShoot()
        {
            animator.SetTrigger(Shoot);
        }

    }
}
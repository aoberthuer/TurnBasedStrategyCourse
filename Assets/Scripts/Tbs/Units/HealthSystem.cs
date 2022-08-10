using System;
using UnityEngine;

namespace tbs.units
{
    public class HealthSystem : MonoBehaviour
    {
        public event Action OnDead;

        [SerializeField] private int health = 100;

        public void Damage(int damageAmount)
        {
            health -= damageAmount;

            if (health < 0)
            {
                health = 0;
            }

            if (health == 0)
            {
                Die();
            }

            Debug.Log(health);
        }

        private void Die()
        {
            OnDead?.Invoke();
        }

    }
}
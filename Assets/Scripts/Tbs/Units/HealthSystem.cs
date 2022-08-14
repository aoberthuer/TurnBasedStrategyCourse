using System;
using UnityEngine;

namespace tbs.units
{
    public class HealthSystem : MonoBehaviour
    {
        public event Action OnDead;
        public event Action OnDamaged;

        [SerializeField] private int health = 100;
        private int _healthMax;

        private void Awake()
        {
            _healthMax = health;
        }

        public void Damage(int damageAmount)
        {
            health -= damageAmount;
            OnDamaged?.Invoke();

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
        
        public float GetHealthNormalized()
        {
            return (float)health / _healthMax;
        }



    }
}
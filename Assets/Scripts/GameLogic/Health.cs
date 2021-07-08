using UnityEngine;

namespace GameLogic
{
    public class Health: MonoBehaviour
    {
        [Min(0)]
        [SerializeField]
        private float currentHealth = 100;

        [Min(0)]
        [SerializeField]
        private float maxHealth = 100;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        
        public void Change(float change)
        {
            currentHealth = Mathf.Clamp(currentHealth + change, 0, maxHealth);
        }
    }
}
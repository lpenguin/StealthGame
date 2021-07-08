using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GameLogic
{
    [RequireComponent(typeof(Image))]
    public class HealthBar: MonoBehaviour
    {
        private Health _playerHealth;
        private Image _image;
        private void Start()
        {
            _playerHealth = GameObject.FindWithTag("Player")?.GetComponent<Health>();
            Assert.IsNotNull(_playerHealth, "Cannot find the player health component");

            _image = GetComponent<Image>();
        }

        private void Update()
        {
            if (_playerHealth.CurrentHealth == 0)
            {
                _image.enabled = false;
                return;
            }

            _image.enabled = true;
            _image.fillAmount = _playerHealth.CurrentHealth / _playerHealth.MaxHealth;
        }
    }
}
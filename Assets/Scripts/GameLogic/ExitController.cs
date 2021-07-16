using UnityEngine;

namespace GameLogic
{
    public class ExitController: MonoBehaviour
    {
        [SerializeField]
        private ContactSensor3D exitSensor;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = Object.FindObjectOfType<GameManager>();
        }

        private void OnEnable()
        {
            if (exitSensor == null)
            {
                return;
            }
            exitSensor.onEnter.AddListener(OnPlayerEnter);
        }
        
        private void OnDisable()
        {
            if (exitSensor == null)
            {
                return;
            }
            exitSensor.onEnter.RemoveListener(OnPlayerEnter);
        }
        
        private void OnPlayerEnter(Collider _)
        {
            _gameManager.LevelComplete();
        }
    }
}
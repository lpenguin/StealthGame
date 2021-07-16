using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameLogic
{
    public class GameManager: MonoBehaviour
    {
        [SerializeField]
        private GameObject menuPanel;
        
        [SerializeField]
        private GameObject levelCompletePanel;
        
        [SerializeField]
        private GameObject levelFailedPanel;
        
        [SerializeField]
        private TMP_Dropdown scenesListDropDown;

        private void Awake()
        {
            scenesListDropDown.options.Clear();
            foreach (var scene in GetBuildScenes())
            {
                scenesListDropDown.options.Add(new TMP_Dropdown.OptionData{ text = scene.name});
            }
        }

        private List<Scene> GetBuildScenes()
        {
            var res = new List<Scene>();
            
            int numScenes = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < numScenes; i++)
            {
                var scene = SceneManager.GetSceneByBuildIndex(i);
                res.Add(scene);
            }

            return res;
        }
        public void LevelComplete()
        {
            levelCompletePanel.gameObject.SetActive(true);
        }
        
        public void Menu()
        {
            menuPanel.gameObject.SetActive(true);
        }
        public void Continue()
        {
            menuPanel.gameObject.SetActive(false);
        }
        
        public void LoadLevel()
        {
            var buildScenes = GetBuildScenes();
            int currentIndex = scenesListDropDown.value;
            if (currentIndex < 0 || currentIndex >= buildScenes.Count)
            {
                return;
            }

            var scene = buildScenes[currentIndex];
            SceneManager.LoadScene(scene.buildIndex);
        }

        public void NextLevel()
        {
            var buildScenes = GetBuildScenes();
            var currentScene = SceneManager.GetActiveScene();
            var nextIndex= buildScenes.IndexOf(currentScene);
            if (nextIndex < 0 || nextIndex >= buildScenes.Count)
            {
                return;
            }
            var scene = buildScenes[nextIndex];
            SceneManager.LoadScene(scene.buildIndex);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        
        public void LevelFailed()
        {
            levelFailedPanel.gameObject.SetActive(true);
        }

    }
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

namespace GameLogic
{
    public class GameManager: MonoBehaviour
    {
        [SerializeField]
        private SceneList scenes;

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
                var sceneName = Path.GetFileNameWithoutExtension(scene);
                scenesListDropDown.options.Add(new TMP_Dropdown.OptionData{ text = sceneName});
            }
        }

        private void Start()
        {
            Unpause();
        }

        public void LevelComplete()
        {
            Pause();
            levelCompletePanel.gameObject.SetActive(true);
        }
        

        public void LevelFailed()
        {
            Pause();
            levelFailedPanel.gameObject.SetActive(true);
        }


        public void Menu()
        {
            Pause();
            menuPanel.gameObject.SetActive(true);
        }

        public void Continue()
        {
            Unpause();
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
            SceneManager.LoadScene(scene);
        }

        public void NextLevel()
        {
            var buildScenes = GetBuildScenes();
            var currentScene = SceneManager.GetActiveScene();
            var nextIndex = buildScenes.IndexOf(currentScene.path) + 1;
            if (nextIndex < 0 || nextIndex >= buildScenes.Count)
            {
                return;
            }
            var scene = buildScenes[nextIndex];
            SceneManager.LoadScene(scene);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        private List<string> GetBuildScenes()
        {
            var res = new List<string>();
            
            foreach(var sceneRef in scenes.scenes)
            {
                res.Add(sceneRef.ScenePath);
            }


            return res;
        }

        private void Pause()
        {
            Time.timeScale = 0;
        }

        private void Unpause()
        {
            Time.timeScale = 1;
        }

    }
}
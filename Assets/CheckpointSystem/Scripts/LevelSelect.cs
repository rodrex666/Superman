using UnityEngine;
using UnityEngine.SceneManagement;

namespace CheckpointSystem.Scripts
{
    public class LevelSelect : MonoBehaviour
    {
        private int randomLevel;
        private string level1;
        private string level2;
        private string level3 = "HCI_VRQuestionnaire";
        private int levelindex = 0;
        // Start is called before the first frame update
        void Start()
        {
            randomLevel = Random.Range(1,3);
            if(randomLevel == 1)
            {
                level1 = "OutdoorScene";
                level2 = "LevelSimple";
            }
            else
            {
                level1 = "LevelSimple";
                level2 = "OutdoorScene";
            }

            SceneManager.LoadScene(level1, LoadSceneMode.Additive);
            Debug.Log(level1 + " loaded");
            levelindex = 1;
        }

        public void NextLevel()
        {
            if(levelindex == 1)
            {
                SceneManager.UnloadSceneAsync(level1);
                SceneManager.LoadScene(level2, LoadSceneMode.Additive);
                Debug.Log(level2 + " loaded");
                levelindex = 2;
            }
            else if(levelindex == 2)
            {
                SceneManager.UnloadSceneAsync(level2);
                SceneManager.LoadScene(level3);
                Debug.Log(level3 + " loaded");
            }
        }
    }
}
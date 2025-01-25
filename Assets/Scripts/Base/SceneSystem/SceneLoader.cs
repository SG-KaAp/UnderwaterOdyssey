using UnityEngine;
using UnityEngine.SceneManagement;

namespace Base.SceneSystem
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    }
}
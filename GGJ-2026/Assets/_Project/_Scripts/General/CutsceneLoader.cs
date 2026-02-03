using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ
{
    public class CutsceneLoader : MonoBehaviour
    {
        [Header("Scene")]
        [SerializeField] private string _sceneToLoad;

        [Header("Timing")]
        [SerializeField] private float _cutsceneDuration = 70f;

        private void Start()
        {
            WaitAndLoad().Forget();
        }

        private async UniTaskVoid WaitAndLoad()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(_cutsceneDuration));

            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}

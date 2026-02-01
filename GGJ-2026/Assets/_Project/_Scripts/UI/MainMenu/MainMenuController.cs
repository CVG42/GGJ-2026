using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGJ
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _configButton;
        [SerializeField] private Button _creditsButton;

        [SerializeField] private string _gameSceneName;
        [SerializeField] GameObject _configScreen;
        [SerializeField] private GameObject _creditsScreen;

        [Header("main Menu screen")]
        [SerializeField] private GameObject _mainMenuScreen;

        [Header("Back buttons")]
        [SerializeField] private Button _settingsBackButton;
        [SerializeField] private Button _creditsBackButton;

        private void Start()
        {
            _startButton.onClick.AddListener(StartGame);
            _exitButton.onClick.AddListener(ExitGame);
            _configButton.onClick.AddListener(OpenSettings);
            _creditsButton.onClick.AddListener(OpenCredits);
            _settingsBackButton.onClick.AddListener(CloseSettings);
            _creditsBackButton.onClick.AddListener(CloseCredits);

            _configScreen.SetActive(false);
            _creditsScreen.SetActive(false);
            _mainMenuScreen.SetActive(true);    
        }

        private void StartGame()
        {
            SceneManager.LoadScene(_gameSceneName);
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void OpenSettings()
        {
            _configScreen.SetActive(true);
            _mainMenuScreen.SetActive(false);
        }

        private void OpenCredits()
        {
            _creditsScreen.SetActive(true);
            _mainMenuScreen.SetActive(false);
        }

        private void CloseCredits()
        {
            _creditsScreen.SetActive(false);
            _mainMenuScreen.SetActive(true);
        }

        private void CloseSettings()
        {
            _configScreen.SetActive(false);
            _mainMenuScreen.SetActive(true);
        }
    }
}

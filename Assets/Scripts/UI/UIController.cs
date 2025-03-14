using SuperGaming.ZombieShooter.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuperGaming.ZombieShooter.Controllers
{
    /// <summary>
    /// this is UI controller class
    /// can be used to manage any type of in game UI
    /// </summary>
    public class UIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI gunStateText;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private GameObject GameOverScreen;
        [SerializeField] private GameObject GameWinScreen;
        public static UIController Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public void SetCurrentBulletText(int remainingBullets, int magSize, bool reloading = false)
        {
            if (reloading)
            {
                gunStateText.text = "Reloading....";
            }
            else
            {
                gunStateText.text = remainingBullets + "/" + magSize;
            }
        }

        public void SetWaveText(int wavecount)
        {
            waveText.text = "Wave " + wavecount;
        }

        private void OnEnable()
        {
            EventManager.OnPlayerKilled += ShowGameOverScreen;
            EventManager.OnAllZombiesKilled += ShowVictoryScreen;
        }

        private void OnDisable()
        {
            EventManager.OnPlayerKilled -= ShowGameOverScreen;
            EventManager.OnAllZombiesKilled -= ShowVictoryScreen;
        }

        private void ShowGameOverScreen()
        {
            GameOverScreen.SetActive(true);
        }

        private void ShowVictoryScreen()
        {
            GameWinScreen.SetActive(true);
        }

        public void OnMenuClicked()
        {
            SceneManager.LoadScene("MenuScene");
            GameplayController.Instance.Reset();
        }
        public void OnRestartClicked()
        {
            GameplayController.Instance.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
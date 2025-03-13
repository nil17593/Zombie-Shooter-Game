using SuperGaming.ZombieShooter.Event;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gunStateText;
    [SerializeField] private TextMeshProUGUI waveText;

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
        Debug.Log("Game Over: Player was killed!");
        // Logic to display Game Over screen
    }

    private void ShowVictoryScreen()
    {
        Debug.Log("Victory: All zombies were killed!");
        // Logic to display Victory screen
    }

    public void OnMenuClicked()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

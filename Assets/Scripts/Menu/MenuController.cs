using UnityEngine.SceneManagement;
using UnityEngine;

namespace SuperGaming.ZombieShooter.Controllers
{
    /// <summary>
    /// this class is attached on Menu scene
    /// handles loading the shop screen and load th game scene
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject shopScreen;

        public void OnPlayButtonClicked()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void OnShopButtonClicked()
        {
            shopScreen.SetActive(true);
        }
    }
}
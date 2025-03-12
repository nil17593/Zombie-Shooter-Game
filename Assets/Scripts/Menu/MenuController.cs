using UnityEngine.SceneManagement;
using UnityEngine;

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

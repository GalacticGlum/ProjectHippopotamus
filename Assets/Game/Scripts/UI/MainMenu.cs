using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingHud;

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync("Game");
        //loadingHud.SetActive(true);
    }
}

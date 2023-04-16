using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /*** Public variables ***/
    public GameObject settingsWindow;

    /*** Private variables ***/
    public void StartGame(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && this.settingsWindow.activeSelf)
        {
            this.CloseSettings();
        }
    }

    public void OpenSettings()
    {
        this.settingsWindow.SetActive(true);
    }

    public void CloseSettings()
    {
        this.settingsWindow.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

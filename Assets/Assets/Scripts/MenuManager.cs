using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;

    public void Start()
    {
        settingsMenu.GetComponent<VolumeSettings>().InitializeVolume(); // Load volume settings at the start
        // Ensure the main menu is active at the start
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        SaveGame.Instance.Save();
        Application.Quit();
    }

    public void LoadLevelMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level Menu");
    }

    public void OpenSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void QuitGame()
    {
        SaveGame.Instance.Save();
        Application.Quit();
    }

    public void LoadLevelMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level Menu");
    }
}

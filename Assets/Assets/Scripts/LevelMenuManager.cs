using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private List<GameObject> levelButtons;
    [SerializeField] private TMPro.TMP_Text stars;
    [SerializeField] private TMPro.TMP_Text deaths;

    [SerializeField] private LevelDataDatabase levelDataDatabase;


    public void LoadLevelMenu()
    {
        levelSelectMenu.SetActive(true);
        settingsMenu.SetActive(false);

        UpdateLevelButtons();
        UpdateStats();

    }

    public void OpenSettingsMenu()
    {
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void UpdateLevelButtons()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i < SaveGame.Instance.saveGameData.level)
            {
                levelButtons[i].GetComponent<Button>().interactable = true;
                levelButtons[i].GetComponent<TMPro.TMP_Text>().text = levelDataDatabase.GetLevelData(i).starsEarned.ToString() + "/" + levelDataDatabase.GetLevelData(i).maxStars.ToString();
            }
            else if (i == SaveGame.Instance.saveGameData.level)
            {
                levelButtons[i].GetComponent<Button>().interactable = true; // Current level is not interactable
            }
            else
            {
                levelButtons[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void UpdateStats()
    {
        int totalStars = levelDataDatabase.TotalStars;
        int maxStars = levelDataDatabase.MaxStars;

        stars.text = totalStars + "/" + maxStars;
        deaths.text = SaveGame.Instance.saveGameData.deaths.ToString();
    }

    public void QuitMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    public void LoadLevel(int level)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level " + level);
    }
}

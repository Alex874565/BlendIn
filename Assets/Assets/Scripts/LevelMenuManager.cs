using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private List<GameObject> levelButtons;
    [SerializeField] private TextMeshProUGUI stars;
    // [SerializeField] private TMPro.TMP_Text deaths;

    [SerializeField] private LevelDataDatabase levelDataDatabase;

    private void Start()
    {
        if (levelDataDatabase == null)
        {
            Debug.LogError("LevelDataDatabase is not assigned in the inspector.");
            return;
        }
        LoadLevelMenu();
        CloseSettingsMenu();
    }

    public void LoadLevelMenu()
    {
        levelSelectMenu.SetActive(true);

        UpdateLevelButtons();
        UpdateStats();

        Debug.Log(SaveGame.Instance.saveGameData.level);

    }

    public void OpenSettingsMenu()
    {
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
    }

    public void UpdateLevelButtons()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            var levelButtonBehaviour = levelButtons[i].GetComponent<LevelButtonBehaviour>();
            if (i < SaveGame.Instance.saveGameData.level)
            {
                levelButtons[i].GetComponentInChildren<Button>().interactable = true;
                levelButtonBehaviour.starsInfo.SetActive(true); // Show stars info for completed levels
                Debug.Log("Level " + i + " is completed. Setting stars info.");
                levelButtonBehaviour.maxStars.text = levelDataDatabase.GetLevelData(i).maxStars.ToString(); // Set max stars for the level
                levelButtonBehaviour.currentStars.text = SaveGame.Instance.saveGameData.starsPerLevel[i].ToString(); // Set stars earned for the level
                levelButtonBehaviour.levelName.text = levelButtonBehaviour.level;
            }
            else if (i == SaveGame.Instance.saveGameData.level)
            {
                levelButtons[i].GetComponentInChildren<Button>().interactable = true; // Current level is not interactable
                levelButtonBehaviour.starsInfo.SetActive(false); // Show stars info for the current level
                levelButtonBehaviour.levelName.text = levelButtonBehaviour.level; // Set level name for the current level
            }
            else
            {
                levelButtons[i].GetComponentInChildren<Button>().interactable = false; // Future levels are not interactable
                levelButtonBehaviour.starsInfo.SetActive(false); // Hide stars info for future levels
                levelButtonBehaviour.levelName.text = "Locked"; // Set level name for future levels
            }
        }
    }

    public void UpdateStats()
    {
        int totalStars = 0;
        foreach (var starsPerLevel in SaveGame.Instance.saveGameData.starsPerLevel)
        {
            totalStars += starsPerLevel;
        }
        int maxStars = levelDataDatabase.MaxStars;

        stars.text = totalStars + "/" + maxStars;
        // deaths.text = SaveGame.Instance.saveGameData.deaths.ToString();
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

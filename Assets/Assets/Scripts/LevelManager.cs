using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private int currentLevelPart;

    [SerializeField] List<GameObject> levelParts;

    [SerializeField] GameObject player;
    [SerializeField] GameObject spawnNext;
    [SerializeField] GameObject spawnPrevious;

    private int maxStars;

    private int currentStars;

    [SerializeField] int level;

    [SerializeField] LevelDataDatabase levelDataDatabase;

    [SerializeField] GameObject finishScreen;

    [SerializeField] TMPro.TMP_Text starsText;

    [SerializeField] GameObject pauseMenu;

    [SerializeField] GameObject deathScreen;

    [SerializeField] UnityEngine.AudioSource sfxAudio;

    [SerializeField] GameObject settingsMenu;

    private void Start()
    {
        Time.timeScale = 1; // Ensure the game starts unpaused
        maxStars = levelDataDatabase.GetLevelData(level).maxStars;
        currentStars = 0;
        starsText.text = currentStars + "/" + maxStars;

        currentLevelPart = 0;
        levelParts[0].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void LoadNextPart()
    {
        if (currentLevelPart < levelParts.Count - 1)
        {
            levelParts[currentLevelPart].SetActive(false);
            currentLevelPart++;
            levelParts[currentLevelPart].SetActive(true);
            player.GetComponent<Rigidbody>().position =  new Vector3(spawnNext.transform.position.x, player.transform.position.y, spawnNext.transform.position.z);
        }
        else
        {
            // If the last part is reached, finish the game
            FinishGame();
        }
    }

    public void LoadPreviousPart()
    {
        if (currentLevelPart > 0)
        {
            levelParts[currentLevelPart].SetActive(false);
            currentLevelPart--;
            levelParts[currentLevelPart].SetActive(true);
            player.GetComponent<Rigidbody>().position = new Vector3(spawnPrevious.transform.position.x, player.transform.position.y, spawnPrevious.transform.position.z);
        }
    }

    public void FinishGame()
    {
        SaveGame.Instance.saveGameData.level = currentLevelPart + 1; // Assuming level parts are sequentially numbered
        SaveGame.Instance.Save();

        levelDataDatabase.SetLevelData(level, currentStars);
        levelDataDatabase.CalculateStars();

        finishScreen.SetActive(true);
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level Menu");
    }

    public void AddStar()
    {
        sfxAudio.Play();
        currentStars += 1;
        if (currentStars > maxStars)
        {
            currentStars = maxStars; // Ensure we don't exceed max stars
        }
        starsText.text = currentStars + "/" + maxStars;
    }

    public void TogglePause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0; // Toggle pause
        pauseMenu.SetActive(Time.timeScale == 0); // Show pause menu if paused
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Time.timeScale = 1; // Resume the game
    }

    public void Death()
    {
        //SaveGame.Instance.saveGameData.deaths++;
        //SaveGame.Instance.Save();
        deathScreen.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

}

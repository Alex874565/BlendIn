using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        maxStars = levelDataDatabase.GetLevelData(level).maxStars;

        currentLevelPart = 0;
        levelParts[0].SetActive(true);
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

    public void Exit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level Menu");
    }

}

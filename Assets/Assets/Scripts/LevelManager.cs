using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] int currentLevelPart;

    [SerializeField] List<GameObject> levelParts;

    [SerializeField] GameObject player;
    [SerializeField] GameObject spawnNext;
    [SerializeField] GameObject spawnPrevious;

    private void Start()
    {
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

}

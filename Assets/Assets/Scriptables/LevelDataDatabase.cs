using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelStarsDatabase", menuName = "ScriptableObjects/LevelDataDatabase", order = 1)]
public class LevelDataDatabase : ScriptableObject
{
    [SerializeField] private int totalStars;
    [SerializeField] private int maxStars;
    [SerializeField] private LevelData[] levelData;

    public int TotalStars => totalStars;
    public int MaxStars => maxStars;

    public LevelData[] LevelDataList => levelData;

    [Serializable]
    public class LevelData
    {
        [SerializeField] public int level;
        [SerializeField] public int maxStars;
        [SerializeField] public int starsEarned;
        [SerializeField] public int deaths;
    }

    public LevelData GetLevelData(int level)
    {
        if (level < 0 || level >= levelData.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Invalid level number.");
        }
        return levelData[level];
    }

    public void SetLevelData(int level, int starsEarned)
    {
        if (level < 0 || level >= levelData.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Invalid level number.");
        }
        if (starsEarned < 0 || starsEarned > maxStars)
        {
            throw new ArgumentOutOfRangeException(nameof(starsEarned), "Stars earned must be between 0 and max stars.");
        }
        levelData[level].starsEarned = starsEarned;
        totalStars += starsEarned;
    }

    public void CalculateStars()
    {
        totalStars = 0;
        foreach (var data in levelData)
        {
            totalStars += data.starsEarned; // Assuming maxStars is the total stars for each level
        }
    }
}

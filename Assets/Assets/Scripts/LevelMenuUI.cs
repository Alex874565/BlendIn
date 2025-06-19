using TMPro;
using UnityEngine;

public class LevelMenuUI : MonoBehaviour
{
    [SerializeField] private LevelDataDatabase levelDatabase;

    private void Start()
    {
        if (levelDatabase == null)
        {
            Debug.LogError("Level database is not assigned.");
            return;
        }

        BindAllLevelContainers();
    }

    private void BindAllLevelContainers()
    {
        for (int i = 0; i < levelDatabase.LevelDataList.Length; i++)
        {
            Transform levelContainer = transform.Find($"Level{i + 1}Container");
            if (levelContainer == null)
            {
                Debug.LogWarning($"Level{i + 1}Container not found under {name}.");
                continue;
            }

            TMP_Text[] texts = levelContainer.GetComponentsInChildren<TMP_Text>();
            if (texts.Length < 2)
            {
                Debug.LogWarning($"Not enough TMP_Text components in {levelContainer.name}");
                continue;
            }

            var levelData = levelDatabase.GetLevelData(i);

            // You may want to assign by name if ordering isn't guaranteed
            foreach (TMP_Text text in texts)
            {
                if (text.name.ToLower().Contains("max"))
                    text.text = levelData.maxStars.ToString();
                else if (text.name.ToLower().Contains("earned"))
                    text.text = levelData.starsEarned.ToString();
            }
        }
    }
}

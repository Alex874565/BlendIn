using UnityEngine;

public class TeleporterBehaviour : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    [SerializeField] TeleporterType teleporterType;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Teleporter collision detected with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with teleporter: " + teleporterType);
            if (teleporterType == TeleporterType.Next)
                levelManager.LoadNextPart();
            else if (teleporterType == TeleporterType.Previous)
                levelManager.LoadPreviousPart();
        }
    }
    
    private enum TeleporterType {
        Next,
        Previous
    }
}

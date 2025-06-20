using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject particles;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collectible collided with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            // Assuming the player has a method to collect items
            other.gameObject.GetComponent<PlayerBehaviour>().CollectItem(gameObject);
            // Instantiate particles at the collectible's position
            if (particles != null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
            }
            Destroy(gameObject); // Destroy the collectible after collection
        }
    }
}

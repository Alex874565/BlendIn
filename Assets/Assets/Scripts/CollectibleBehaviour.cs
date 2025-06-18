using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collectible collided with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            // Assuming the player has a method to collect items
            other.gameObject.GetComponent<PlayerBehaviour>().CollectItem(gameObject);
            Destroy(gameObject); // Destroy the collectible after collection
        }
    }
}

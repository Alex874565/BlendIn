using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehaviour : MonoBehaviour
{
    private Collider spikeCollider;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle player collision with spike
            Debug.Log("Player hit by spike!");
            // You can add more logic here, like reducing health or triggering an animation
        }
    }
}

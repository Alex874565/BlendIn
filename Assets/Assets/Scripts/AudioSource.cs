using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSource : MonoBehaviour
{
    private static AudioSource instance;

    private void Awake()
    {
        // If another instance already exists, destroy this one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the active instance
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
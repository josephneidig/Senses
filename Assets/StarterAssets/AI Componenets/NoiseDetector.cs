using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseDetector : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (AudioManager.audioManager.isPlaying())
        {
            Debug.Log("Detected");
        }
    }
}

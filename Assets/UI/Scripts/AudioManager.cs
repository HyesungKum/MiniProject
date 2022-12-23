using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().value = audioSource.volume;
    }

    public void _audioController()
    {
        audioSource.volume = GetComponent<Slider>().value;
    }
}

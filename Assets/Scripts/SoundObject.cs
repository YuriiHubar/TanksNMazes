using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour {
    public AudioClip Sound;
    public float Volume = 0.33f;

    // Use this for initialization
    void Start ()
    {
        GetComponent<AudioSource>().PlayOneShot(Sound, Volume);
        Destroy(gameObject, Sound.length);
    }
}

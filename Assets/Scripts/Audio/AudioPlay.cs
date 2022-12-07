using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    AudioSource killedSound;
    // Start is called before the first frame update
    void Start()
    {
        killedSound = GetComponent<AudioSource>();
		killedSound.volume = StateManager.voulumeLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void play_sound() {
        killedSound.Play();
    }
}

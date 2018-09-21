using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerSighting : MonoBehaviour {

    public Vector3 position = new Vector3(1000f, 1000f, 1000f);         // The last global sighting of the player.
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);    // The default position if the player is not in sight.


    public float lightHighIntensity = 0.25f;                            // The directional light's intensity when the alarms are off.
    public float lightLowIntensity = 0f;                                // The directional light's intensity when the alarms are on.
    public float fadeSpeed = 7f;                                        // How fast the light fades between low and high intensity.
    public float musicFadeSpeed = 1f;                                   // The speed at which the 


    private AlarmLight alarm;                                           // Reference to the AlarmLight script.
    private Light mainLight;                                            // Reference to the main light.
    private AudioSource panicAudio;                                     // Reference to the AudioSource of the panic msuic.
    private AudioSource[] sirens;                                       // Reference to the AudioSources of the megaphones.


    private AudioSource audioSource;



    void Awake () {

        //  Game Controller AudioSource
        audioSource = GetComponent<AudioSource>();


        // Setup the reference to the alarm light.
        alarm = GameObject.FindGameObjectWithTag(Tags.alarm).GetComponent<AlarmLight>();

        // Setup the reference to the main directional light in the scene.
        mainLight = GameObject.FindGameObjectWithTag(Tags.mainLight).GetComponent<Light>();

        // Setup the reference to the additonal audio source.
        panicAudio = transform.Find("secondaryMusic").GetComponent<AudioSource>();

        // Find an array of the siren gameobjects.
        GameObject[] sirenGameObjects = GameObject.FindGameObjectsWithTag(Tags.siren);

        // Set the sirens array to have the same number of elements as there are gameobjects.
        sirens = new AudioSource[sirenGameObjects.Length];

        // For all the sirens allocate the audio source of the gameobjects.
        for (int i = 0; i < sirens.Length; i++)
        {
            sirens[i] = sirenGameObjects[i].GetComponent<AudioSource>();
        }
    
    }
	
	// Update is called once per frame
	void Update () {

        // Switch the alarms and fade the music.
        SwitchAlarms();
        MusicFading();

    
    }


    private void SwitchAlarms()
    {
        // Set the alarm light to be on or off.
        //alarm.alarmOn = position != resetPosition;
        alarm.alarmOn = !Vector3Equal(position, resetPosition);

        // Create a new intensity.
        float newIntensity;

        // If the position is not the reset position...
        if (!Vector3Equal(position, resetPosition))
            // ... then set the new intensity to low.
            newIntensity = lightLowIntensity;
        else
            // Otherwise set the new intensity to high.
            newIntensity = lightHighIntensity;

        // Fade the directional light's intensity in or out.
        mainLight.intensity = Mathf.Lerp(mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);

        // For all of the sirens...
        for (int i = 0; i < sirens.Length; i++)
        {
            // ... if alarm is triggered and the audio isn't playing, then play the audio.
            if (!Vector3Equal(position, resetPosition) && !sirens[i].isPlaying)
                sirens[i].Play();
            // Otherwise if the alarm isn't triggered, stop the audio.
            //else if (position == resetPosition)
            else if (Vector3Equal(position, resetPosition))
                sirens[i].Stop();
        }
    }


    private void MusicFading()
    {

        // If the alarm is not being triggered...
        if (!Vector3Equal(position, resetPosition))
        {
            // ... fade out the normal music...
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, musicFadeSpeed * Time.deltaTime);

            // ... and fade in the panic music.
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
        }
        else
        {
            // Otherwise fade in the normal music and fade out the panic music.
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
        }
    }

    private bool Vector3Equal (Vector3 a, Vector3 b) {
        return Vector3.SqrMagnitude(a - b) < Mathf.Epsilon;
    }

}

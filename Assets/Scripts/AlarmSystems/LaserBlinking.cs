using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlinking : MonoBehaviour {

    [SerializeField]
    private float onTime;            // Amount of time in seconds the laser is on for.
    [SerializeField]
    private float offTime;           // Amount of time in seconds the laser is off for.


    private float timer;            // Timer to time the laser blinking.

    private Renderer rendererComponent;
    private Light lightComponent;

    private void Awake() {

        rendererComponent = GetComponent<Renderer>();
        lightComponent = GetComponent<Light>();

    }

    // Update is called once per frame
    void Update () {

        // Increment the timer by the amount of time since the last frame.
        timer += Time.deltaTime;

        // If the beam is on and the onTime has been reached...
        if (rendererComponent.enabled && timer >= onTime) {
            // Switch the beam.
            SwitchBeam();
        }

        // If the beam is off and the offTime has been reached...
        if (!rendererComponent.enabled && timer >= offTime) {
            // Switch the beam.
            SwitchBeam();
        }

    }

    private void SwitchBeam() {
        // Reset the timer.
        timer = 0f;

        // Switch whether the beam and light are on or off.
        rendererComponent.enabled = !rendererComponent.enabled;
        lightComponent.enabled = !lightComponent.enabled;
    }

}

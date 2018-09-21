using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour {

    [SerializeField]
    private float fadeSpeed = 2f;            // How fast the light fades between intensities.
    [SerializeField]
    private float highIntensity = 2f;        // The maximum intensity of the light whilst the alarm is on.
    [SerializeField]
    private float lowIntensity = 0.5f;       // The minimum intensity of the light whilst the alarm is on.
    [SerializeField]
    private float changeMargin = 0.2f;       // The margin within which the target intensity is changed.

    public bool alarmOn;                    // Whether or not the alarm is on.
    
    
    private float targetIntensity;          // The intensity that the light is aiming for currently.

    private Light alarmLight;


	void Awake () {

        alarmLight = GetComponent<Light>();

        // When the level starts we want the light to be "off".
        alarmLight.intensity = 0f;

        // When the alarm starts for the first time, the light should aim to have the maximum intensity.
        targetIntensity = highIntensity;
    }
	
	void Update () {

        // If the light is on...
        if (alarmOn) {
            // ... Lerp the light's intensity towards the current target.
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, targetIntensity, fadeSpeed * Time.deltaTime);

            // Check whether the target intensity needs changing and change it if so.
            CheckTargetIntensity();
        }
        else {
            // Otherwise fade the light's intensity to zero.
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, 0f, fadeSpeed * Time.deltaTime);
        }

    }

    private void CheckTargetIntensity() {
        // If the difference between the target and current intensities is less than the change margin...
        if (Mathf.Abs(targetIntensity - alarmLight.intensity) < changeMargin)
        {
            // ... if the target intensity is high...
            if (targetIntensity >= highIntensity) {
                // ... then set the target to low.
                targetIntensity = lowIntensity;
            }
            else {
                // Otherwise set the targer to high.
                targetIntensity = highIntensity;
            }
        }
    }


}

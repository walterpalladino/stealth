using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneFadeInOut : MonoBehaviour {

    [SerializeField]
    private float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.

    
    private bool sceneStarting = true;      // Whether or not the scene is still fading in.
    private Image panel;


    void Awake() {

        panel = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update () {
        // If the scene is starting...
        if (sceneStarting) {
            // ... call the StartScene function.
            StartScene();
        }
    }

    private void FadeToClear()
    {
        // Lerp the colour of the texture between itself and transparent.
        panel.color = Color.Lerp(panel.color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    private void FadeToBlack()
    {
        // Lerp the colour of the texture between itself and black.
        panel.color = Color.Lerp(panel.color, Color.black, fadeSpeed * Time.deltaTime);
    }


    public void StartScene()
    {
        // Fade the texture to clear.
        FadeToClear();

        // If the texture is almost clear...
        if (panel.color.a <= 0.05f)
        {
            // ... set the colour to clear and disable the GUITexture.
            panel.color = Color.clear;
            panel.enabled = false;

            // The scene is no longer starting.
            sceneStarting = false;
        }
    }


    public void EndScene()
    {
        // Make sure the texture is enabled.
        panel.enabled = true;

        // Start fading towards black.
        FadeToBlack();

        // If the screen is almost black...
        if (panel.color.a >= 0.95f)
        {
            // ... reload the level.
            SceneManager.LoadScene(0);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPlayerDetection : MonoBehaviour {

    private GameObject player;                             // Reference to the player.
    private LastPlayerSighting lastPlayerSighting;      // Reference to the global last sighting of the player.

    private Renderer rendererComponent;


    void Awake() {

        rendererComponent = GetComponent<Renderer>();

        // Setting up references.
        player = GameObject.FindGameObjectWithTag(Tags.player);
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
    }


    void OnTriggerStay(Collider other) {
        // If the beam is on...
        if (rendererComponent.enabled) {

            // ... and if the colliding gameobject is the player...
            if (other.gameObject == player) {
                // ... set the last global sighting of the player to the colliding object's position.
                lastPlayerSighting.position = other.transform.position;
            }

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float health = 100f;                         // How much health the player has left.
    public float resetAfterDeathTime = 5f;              // How much time from the player dying to the level reseting.
    public AudioClip deathClip;                         // The sound effect of the player dying.


    private Animator anim;                              // Reference to the animator component.
    private PlayerMovement playerMovement;          // Reference to the player movement script.
    private HashIDs hash;                           // Reference to the HashIDs.
    private SceneFadeInOut sceneFadeInOut;          // Reference to the SceneFadeInOut script.
    private LastPlayerSighting lastPlayerSighting;  // Reference to the LastPlayerSighting script.
    private float timer;                                // A timer for counting to the reset of the level once the player is dead.
    private bool playerDead;                            // A bool to show if the player is dead or not.

    private AudioSource audioSource;


    void Awake() {
    
        // Setting up the references.
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFadeInOut>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
    
    }
	
	// Update is called once per frame
	void Update () {
        // If health is less than or equal to 0...
        if (health <= 0f)
        {
            // ... and if the player is not yet dead...
            if (!playerDead) {
                // ... call the PlayerDying function.
                PlayerDying();
            }
            else {
                // Otherwise, if the player is dead, call the PlayerDead and LevelReset functions.
                PlayerDead();
                LevelReset();
            }
        }
    }

    void PlayerDying()
    {
        // The player is now dead.
        playerDead = true;

        // Set the animator's dead parameter to true also.
        anim.SetBool(hash.deadBool, playerDead);

        // Play the dying sound effect at the player's location.
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
    }


    void PlayerDead()
    {
        // If the player is in the dying state then reset the dead parameter.
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == hash.dyingState) {
            anim.SetBool(hash.deadBool, false);
        }

        // Disable the movement.
        anim.SetFloat(hash.speedFloat, 0f);
        playerMovement.enabled = false;

        // Reset the player sighting to turn off the alarms.
        lastPlayerSighting.position = lastPlayerSighting.resetPosition;

        // Stop the footsteps playing.
        audioSource.Stop();
    }


    void LevelReset()
    {
        // Increment the timer.
        timer += Time.deltaTime;

        //If the timer is greater than or equal to the time before the level resets...
        if (timer >= resetAfterDeathTime) {
            // ... reset the level.
            sceneFadeInOut.EndScene();
        }
    }


    public void TakeDamage(float amount)
    {
        // Decrement the player's health by amount.
        health -= amount;
    }

}

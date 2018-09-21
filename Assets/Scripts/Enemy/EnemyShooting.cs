using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

    [SerializeField]
    private float maximumDamage = 120f;                  // The maximum potential damage per shot.
    [SerializeField]
    private float minimumDamage = 45f;                   // The minimum potential damage per shot.
    [SerializeField]
    private AudioClip shotClip;                          // An audio clip to play when a shot happens.
    [SerializeField]
    private float flashIntensity = 3f;                   // The intensity of the light when the shot happens.
    [SerializeField]
    private float fadeSpeed = 10f;                       // How fast the light will fade after the shot.


    private Animator anim;                              // Reference to the animator.
    private HashIDs hash;                           // Reference to the HashIDs script.
    private LineRenderer laserShotLine;                 // Reference to the laser shot line renderer.
    private Light laserShotLight;                       // Reference to the laser shot light.
    private SphereCollider col;                         // Reference to the sphere collider.
    private Transform player;                           // Reference to the player's transform.
    private PlayerHealth playerHealth;              // Reference to the player's health.
    private bool shooting;                              // A bool to say whether or not the enemy is currently shooting.
    private float scaledDamage;                         // Amount of damage that is scaled by the distance from the player.



    void Awake () {

        // Setting up the references.
        anim = GetComponent<Animator>();
        laserShotLine = GetComponentInChildren<LineRenderer>();
        laserShotLight = laserShotLine.gameObject.GetComponent<Light>();
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

        // The line renderer and light are off to start.
        laserShotLine.enabled = false;
        laserShotLight.intensity = 0f;

        // The scaledDamage is the difference between the maximum and the minimum damage.
        scaledDamage = maximumDamage - minimumDamage;
    
    }
	
	// Update is called once per frame
	void Update () {

        // Cache the current value of the shot curve.
        float shot = anim.GetFloat(hash.shotFloat);

        // If the shot curve is peaking and the enemy is not currently shooting...
        if (shot > 0.5f && !shooting) {
            // ... shoot
            Shoot();
        }

        // If the shot curve is no longer peaking...
        if (shot < 0.5f) {
            // ... the enemy is no longer shooting and disable the line renderer.
            shooting = false;
            laserShotLine.enabled = false;
        }

        // Fade the light out.
        laserShotLight.intensity = Mathf.Lerp(laserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);
    
    }


    void OnAnimatorIK(int layerIndex) {
        // Cache the current value of the AimWeight curve.
        float aimWeight = anim.GetFloat(hash.aimWeightFloat);

        // Set the IK position of the right hand to the player's centre.
        anim.SetIKPosition(AvatarIKGoal.RightHand, player.position + Vector3.up * 1.5f);

        // Set the weight of the IK compared to animation to that of the curve.
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);
    }


    void Shoot() {

        // The enemy is shooting.
        shooting = true;

        // The fractional distance from the player, 1 is next to the player, 0 is the player is at the extent of the sphere collider.
        float fractionalDistance = (col.radius - Vector3.Distance(transform.position, player.position)) / col.radius;

        // The damage is the scaled damage, scaled by the fractional distance, plus the minimum damage.
        float damage = scaledDamage * fractionalDistance + minimumDamage;

        // The player takes damage.
        playerHealth.TakeDamage(damage);

        // Display the shot effects.
        ShotEffects();
    }

    void ShotEffects() {
        // Set the initial position of the line renderer to the position of the muzzle.
        laserShotLine.SetPosition(0, laserShotLine.transform.position);

        // Set the end position of the player's centre of mass.
        laserShotLine.SetPosition(1, player.position + Vector3.up * 1.5f);

        // Turn on the line renderer.
        laserShotLine.enabled = true;

        // Make the light flash.
        laserShotLight.intensity = flashIntensity;

        // Play the gun shot clip at the position of the muzzle flare.
        AudioSource.PlayClipAtPoint(shotClip, laserShotLight.transform.position);
    }

}

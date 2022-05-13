using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryGunShoot : MonoBehaviour
{
    public int damagePerShot = 20;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;        // The time between each shot.
    public float range = 100f;                      // The distance the gun can fire.

    float timer;                                    // A timer to determine when to fire.
    Ray shootRay = new Ray();                       // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    ParticleSystem gunParticles;                    // Reference to the particle system.
    LineRenderer gunLine;                           // Reference to the line renderer.
    float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.


    void Awake()
    {
        // Set up the references.
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire...
        if ((Input.GetMouseButton(1) && timer >= timeBetweenBullets && Time.timeScale != 0))
        {
            // ... shoot the gun.
            Shoot();
        }

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            // ... disable the effects.
            DisableEffects();
        }
    }


    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
    }


    void Shoot()
    {
        // Reset the timer.
        timer = 0f;

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.Play();

        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        // If it hits something, set the second position of the line renderer to the point the raycast hit, otherwise, 
        // set the second position of the line renderer to the maximal raycast range.

        //In other words, we use raycast to determine where the ray ends, i.e if there is a rock in the front, the ray will be stopped by the rock,
        //otherwise, it will reach to the maximal raycast range

        int layerMask = 1 << 8; //bit shift the index of the layer (8) to get a bit mask

        layerMask = ~layerMask; //this cast rays only against colliders in layer 8, as we want to collide against everything except layer 8.
        //LayerMask shootable_layerMask = LayerMask.GetMask("Default"); //get if not working, try  with layerMask = ~layerMask;

        // Does the ray intersect any objects excluding the player layer ?

        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out shootHit, Mathf.Infinity, layerMask)) //if there is a rock in the front (...i.e. if it hits something)
        {
            gunLine.SetPosition(1, transform.TransformDirection(Vector3.forward) * shootHit.distance); //ray should be stopped by the object being hit
            Debug.Log("Did Hit" + " " + shootHit.collider.gameObject.name + " " + shootHit.collider.gameObject.tag);

            if (shootHit.collider.gameObject.tag == "enemy")
            {
                Destroy(shootHit.collider.gameObject); //here we destroy the object which the laser impacted with

            }
        }
        else
        {
            gunLine.SetPosition(1, Vector3.positiveInfinity); //otherwise, it will reach to the maximal raycast range (set the second position of the line renderer to the maximal raycast range.)
            Debug.Log("Did not Hit");
        }
    }
}

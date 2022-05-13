using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCollision : MonoBehaviour
{
    public GameObject explosionParticlesPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the shell if it hits something (e.g. rock, ground, enemytank, etc.). 
        // If the shell hits enemy tank, the enemy tank will also be destroyed.
        
        // This just destroys one tank at the same time. Imlement a foreach to be able to destroy more than one tank at the same time.
        // Destroy the shell if it hits something (e.g. rock, ground, enemytank, etc.). 
        if ((collision.gameObject.tag == "rocks") || (collision.gameObject.name == "ground") || (collision.gameObject.name == "enemytank"))
        {
            Debug.Log("Collision detected, shell destroyed");
            Instantiate(explosionParticlesPrefab, collision.gameObject.transform.position, Quaternion.identity); //activate particle system when the shell hits something
            Destroy(collision.collider); //here we destroy the shell
            Destroy(collision.gameObject); //here we destroy the object which the shell impacted with
        }

        // If the shell hits enemy tank, the enemy tank will also be destroyed 
        if ((collision.gameObject.tag == "enemytank"))
        {
            Debug.Log("Collision detected, enemy tank destroyed");
            //Destroy(gameObject); //here we destroy the object which the shell impacted with
            Instantiate(explosionParticlesPrefab, collision.gameObject.transform.position, Quaternion.identity); //activate particle system when the shell hits something
            Destroy(collision.collider); //here we destroy the shell
            Destroy(collision.gameObject); //here we destroy the object which the shell impacted with
        }
    }

}

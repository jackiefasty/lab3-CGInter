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
        Debug.Log("Function called. Hit " + collision.gameObject.name + "-" + collision.gameObject.tag);


        //we don't need other checks to destroy the shell, because onCollisionEnter is only called when there is a collision and the shell should be destroyed whenever it hits anything
        GameObject explosion = (GameObject)Instantiate(explosionParticlesPrefab, collision.gameObject.transform.position, explosionParticlesPrefab.transform.rotation); //activate particle system when the shell hits something
         

        // If the shell hits enemy tank, the enemy tank will also be destroyed 
        if ((collision.gameObject.tag == "enemy"))
        {
            Destroy(collision.gameObject); //here we destroy the object which the shell impacted with
            
        }

        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier); //destroy the particle system as  explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier
        Destroy(collision.collider); //here we destroy the shell
    }

}

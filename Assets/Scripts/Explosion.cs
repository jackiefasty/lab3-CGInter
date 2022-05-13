using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public GameObject explosionParticlesPrefab;

    // Use this for initialization
    void Start()
    {

    }

    

    public void OnTriggerEnter(Collider other)
    {
        if (explosionParticlesPrefab)
        {
            GameObject explosion = (GameObject)Instantiate(explosionParticlesPrefab, transform.position, explosionParticlesPrefab.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        }

        //Destroy(gameObject);
    }
}

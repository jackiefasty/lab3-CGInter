using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 1.0f;       // Adjust the speed for the application.

    Transform player;               // Reference to the player's position.
    bool stopped = false;

    void Awake()
    {
        // Set up the references.
        player = GameObject.Find("Tank").transform;
    }

    void Update()
    {
        if (!stopped)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            Vector3 newPos = new Vector3(player.position.x, 0f, player.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, step);
            transform.LookAt(player);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                // Swap the position of the cylinder.
                stopped = true;
                Destroy(this.gameObject);
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RadarAlarm : MonoBehaviour
{
    public GameObject enemy;

    private LineRenderer lineRenderer;
    private GameObject spawnPoint;

    [Range(0.1f, 100f)]
    public float radius = 30.0f;

    [Range(3, 256)]
    public int numSegments = 128;

    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        spawnPoint = GameObject.Find("SpawnPoint");
    }

    void Start()
    {
        DoRenderer();
    }

    public void DoRenderer()
    {         
        Color c1 = new Color(0.5f, 0.5f, 0.5f, 1);
        lineRenderer.material = Resources.Load<Material>("Materials/Save");
        lineRenderer.startColor = c1;
        lineRenderer.endColor = c1;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = numSegments + 1;                
        lineRenderer.useWorldSpace = false;

        float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
        float theta = 0f;

        for (int i = 0; i < numSegments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    public void OnTriggerEnter(Collider other) //to make Radar detect the tank
    { //if tank enters the detecting range (presented by capsule collider) color of line changes to red
        if (other.name == "Tank")
        {
            //we need to change every time the color of the material, not the startColor or endColor of the lineRenderer
            lineRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 1); //ring turns red
        }

        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", 3f, 3f);
    }

    public void OnTriggerExit(Collider other) //to make Radar detect the tank
    { //if tank exits the detecting range (presented by capsule collider) color of line changes to green
        
        if (other.name == "Tank") //when the collider is the Tank, then means that the Radar is colliding with the Tank
        {
            //we need to change every time the color of the material, not the startColor or endColor of the lineRenderer
            lineRenderer.material.color = new Color(0.0f, 1.0f, 0.0f, 1); //ring turns green, we create new color each time to be used in public void methods
        }

        CancelInvoke("Spawn");
    }


    void Spawn()
    {
        if (enemy != null)
        {
            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            GameObject enemytank = Instantiate(enemy, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }        
    }
}

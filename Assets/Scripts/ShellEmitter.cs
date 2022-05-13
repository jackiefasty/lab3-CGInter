using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEmitter : MonoBehaviour {

    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.

    private GameObject shellPrefab;
    private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
    private bool m_Fired;                       // Whether or not the shell has been launched with this button press.
    private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.

    public Vector3 sizeChange;                  // ADDED: 3D vector to change the size of the shell


    void Awake()
    {
        shellPrefab = Resources.Load<GameObject>("Prefabs/Shell");
    }

    
    void Start () {
        // The rate that the launch force charges up is the range of possible forces by the max charge time.
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }
	
	// Update is called once per frame
	void Update () {
        // If the max force has been exceeded and the shell hasn't yet been launched...
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // ... use the max force and launch the shell.
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        // Otherwise, if the fire button has just started being pressed...
        else if (Input.GetMouseButtonDown(0))
        {
            // ... reset the fired flag and reset the launch force.
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;
        }
        // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
        else if (Input.GetMouseButton(0) && !m_Fired)
        {
            // Increment the launch force and update the slider.
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
        }
        // Otherwise, if the fire button is released and the shell hasn't been launched yet...
        else if (Input.GetMouseButtonUp(0) && !m_Fired)
        {
            // ... launch the shell.
            Fire();
        }
    }

    private void Fire()
    {
        // Set the fired flag so only Fire is only called once.
        m_Fired = true;

        if (shellPrefab != null)
        {
            // Create an instance of the shell and store a reference to it's rigidbody.
            GameObject shellInstance = Instantiate(shellPrefab, transform.position, transform.rotation);

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.GetComponent<Rigidbody>().velocity = m_CurrentLaunchForce * transform.forward;

            //ADDED: defining new Vector3 size to get new size of shell
            sizeChange = new Vector3(0.8f, 0.8f, 0.8f);
            shellInstance.transform.localScale = shellInstance.transform.localScale - sizeChange;

            // Set the shell's life time to be 3 second. After that, the shell will be destroyed.
            Destroy(shellInstance, 3f);
        }
    }
}

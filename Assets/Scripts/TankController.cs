using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankController : MonoBehaviour
{
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public float m_WheelRotateSpeed = 90f;
    public float m_turretDelaySpeed = 1.2f;

    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private Vector3 m_MouseInputValue;          // The current value of the mouse input
    private int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    private List<GameObject> m_wheels = new List<GameObject>();
    private GameObject m_turret;
    private float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    private float turn;
    private Vector3 tankToMouseSmooth;

    private void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Ground");

        m_Rigidbody = GetComponent<Rigidbody>();

        Transform[] children = GetComponentsInChildren<Transform>();
        for (var i = 0; i < children.Length; i++)
        {
            // Get all wheels in the children
            if (children[i].name.Contains("wheel"))
            {
                m_wheels.Add(children[i].gameObject);
            }

            // Get turret
            if (children[i].name.Contains("Turret"))
            {
                m_turret = children[i].gameObject;
            }
        }


    }

    // Start is called before the first frame update
    private void Start()
    {
        m_MovementAxisName = "Vertical";
        m_TurnAxisName = "Horizontal";
        turn = 0;


    }

    private void Update()
    {
        // Store the value of both input axes.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
        m_MouseInputValue = Input.mousePosition;
    }

    private void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
        Turn();
        RotateWheels();
        RotateTurret();
    }


    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }

    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }


    // Rotate tank wheels. 
    private void RotateWheels()
    {

        // If tank is moving
        if (m_MovementInputValue != 0)
        {
            // Get direction and rotate based if movement is forward or backward
            // When tank moves forward, the wheels should rotate forward; When tank moves backwards, the wheels should rotate backwards.
            if (m_MovementInputValue < 0)
                turn += (-1) * Time.deltaTime * m_WheelRotateSpeed;
            else
                turn += Time.deltaTime * m_WheelRotateSpeed;


            if (turn > 360.0f)
            {
                turn = 0.0f;
            }

            // You should use quaternion to rotate the wheels.
            Quaternion turnRotation = Quaternion.Euler(turn, 0f, 0f);


            // Do it for every wheel
            foreach (GameObject wheel in m_wheels)
            {
                // Note that there is no rigidbody attached to the wheels, you might think of using transform.localRotation (as shown below) instead of using rigidbody.rotation to rotate the wheels.
                wheel.transform.localRotation = turnRotation;
            }

            //Debug.Log(turn);
        }

    }

    private void RotateTurret()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the turret to the point on the floor the raycast from the mouse hit.
            Vector3 tankToMouse = floorHit.point - m_turret.transform.position;

            // The turret should rotate with y value to be 0, otherwise the turret will also rotate up and down and clip into the body of the tank
            tankToMouse.y = 0;

            // Creating delay so that the movement is smooth
            tankToMouseSmooth = Vector3.Lerp(tankToMouseSmooth, tankToMouse, Time.deltaTime * m_turretDelaySpeed);

            m_turret.transform.rotation = Quaternion.LookRotation(tankToMouseSmooth);
        }
    }
}
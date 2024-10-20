using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingToTrash : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 startingPosition;
    public Quaternion startingRotation;


    public float moveSpeed = 5f;
    public Vector3 screenPosition;
    public Vector3 direction;
    bool shouldMove = false;
    // Update is called once per frame
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is the target you want to trigger the action for
        if (other.CompareTag("obstacle")) // Make sure your target has the tag "Target"
        {
            // Call your action here
            Destroy(other.gameObject);
            shouldMove = false;
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            Debug.Log("COLLIDING");
        }
    }
    void Update()
    {


        // Set the z coordinate to the distance from the camera
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            screenPosition = new Vector3(600, 768 / 2, 0);
            screenPosition.z = mainCamera.nearClipPlane; // Or another appropriate distance

            // Convert the screen position to a world position
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(screenPosition);

            // Calculate the direction to move towards
            direction = (targetPosition - transform.position).normalized;
            shouldMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightShift))
        {
            screenPosition = new Vector3(750, 768 / 2, 0);
            screenPosition.z = mainCamera.nearClipPlane; // Or another appropriate distance

            // Convert the screen position to a world position
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(screenPosition);

            // Calculate the direction to move towards
            direction = (targetPosition - transform.position).normalized;
            shouldMove = true;
        }


        // Move the character towards the target direction
        if (shouldMove)
        {
            MoveCharacter(direction);
        }





    }


    void MoveCharacter(Vector3 direction)
    {
        // Move in the direction
        transform.position += direction * moveSpeed * Time.deltaTime;


        // Optionally, rotate the character to face the direction
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Smooth rotation
        }
    }
}

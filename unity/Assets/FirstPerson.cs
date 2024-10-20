using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPS : MonoBehaviour
{
    public Camera playerCamera;
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    private CharacterController characterController;
    private float rotationX = 0;
    // Speed of rotation

    private Vector3 targetScreenPosition = new(300, 300, 0); // Target screen position


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();

    }
    /*void lookAtTrash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Get the center of the screen as the target position
            targetScreenPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            // Start looking at the target position
            LookAtTarget();
        }

        // Rotate towards the target position if needed

    }*/
    void LookAtTarget()
    {
        // Convert screen position to world position
        Vector3 targetWorldPosition = playerCamera.ScreenToWorldPoint(new Vector3(targetScreenPosition.x, targetScreenPosition.y, playerCamera.nearClipPlane));

        // Calculate direction to the target position
        Vector3 direction = (targetWorldPosition - transform.position).normalized;

        // Create a rotation that looks in that direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);

        // Optional: Stop looking once we are close enough to the target direction

    }
    void HandleLook()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    void HandleMovement()
    {
        // Get horizontal and vertical input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveY = 0;

        // Allow vertical movement
        if (Input.GetKey(KeyCode.Space)) // Move up
        {
            moveY = 1;
        }
        else if (Input.GetKey(KeyCode.LeftControl)) // Move down
        {
            moveY = -1;
        }

        // Create movement vector
        Vector3 move = new Vector3(moveX, moveY, moveZ);
        move = transform.TransformDirection(move); // Convert to world space

        // Move the character
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }
}
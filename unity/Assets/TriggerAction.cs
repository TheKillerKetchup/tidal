using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is the target you want to trigger the action for
        if (other.CompareTag("Target")) // Make sure your target has the tag "Target"
        {
            // Call your action here
            StopMovement();
        }
    }

    private void PerformAction()
    {
        // Action to perform when entering the trigger
        other.
        // Add your custom logic here
    }
}

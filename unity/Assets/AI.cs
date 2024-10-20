using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private Transform target2;
    [SerializeField] private Transform target3;
    public Vector3 startingPosition;
    public Quaternion startingRotation;
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
            agent.isStopped = true;
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            Debug.Log("COLLIDING");
            agent.isStopped = false;
        }
    }
    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
        else if (target2 != null)
        {
            agent.SetDestination(target2.position);
        }
        else if (target3 != null)
        {
            agent.SetDestination(target3.position);
        }
    }

    // Update is called once per frame

}
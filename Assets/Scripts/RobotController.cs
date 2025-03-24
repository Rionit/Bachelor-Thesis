using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        GoTo(Vector3.zero);
    }

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                // Destination reached
                animator.SetTrigger("TrStop");
            }
        }
    }

    public void GoTo(Vector3 destination)
    {
        agent.isStopped = false;
        agent.SetDestination(destination);
        animator.SetTrigger("TrDrive");
    }

    public void Stop()
    {
        agent.isStopped = true;
        animator.SetTrigger("TrStop");
    }
    
}

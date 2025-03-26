using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public LayerMask layersToHit;
    
    private NavMeshAgent agent;
    private Animator animator;
    private Camera cam;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        
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

        if (Input.GetMouseButton(0))
        {
            RaycastPosition(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            RaycastPosition(Input.GetTouch(0).position);
        }
    }

    private void RaycastPosition(Vector3 screenPosition)
    {
        Ray ray = cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 5);
            Debug.Log(hit.collider.gameObject.name + " was hit!");
            transform.position = hit.point;
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

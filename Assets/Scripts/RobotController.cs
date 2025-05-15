/*
 * Author: Filip Doležal
 * Date: 24.3.2025
 * 
 * Description: Handles the robot and its placement on screen touch.
 *              
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); 

        if (!stateInfo.IsTag("Transitioning")) 
        {
            // If moving, play drive animation
            if (agent.velocity == Vector3.zero && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!stateInfo.IsName("Idle")) 
                {
                    animator.ResetTrigger("TrDrive");
                    animator.SetTrigger("TrStop");
                }
            }
            // Otherwise be idle and stop
            else
            {
                if (!stateInfo.IsName("Armature|Drive")) 
                {
                    animator.ResetTrigger("TrStop");
                    animator.SetTrigger("TrDrive");
                }
            }
        }

        // MOUSE
        if (Input.GetMouseButton(0))
        {
            RaycastPosition(Input.mousePosition);
        }
        // TOUCH
        else if (Input.touchCount > 0)
        {
            RaycastPosition(Input.GetTouch(0).position);
        }

        // Next location
        GameObject next = NavigationManager.Instance.GetNextLocationInPath();
        if (next != null && agent.destination != next.transform.position) GoTo(next.transform.position);
    }

    private void RaycastPosition(Vector3 screenPosition)
    {
        Ray ray = cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit))
        {
            // Only raycast through Canvas can be allowed 
            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameObject hoveredObject = EventSystem.current.currentSelectedGameObject;
                if (hoveredObject != null && hoveredObject.GetComponent<Canvas>() == null)
                {
                    return;
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 5);
            // Debug.Log(hit.collider.gameObject.name + " was hit!");
            agent.Warp(hit.point);
        }
    }

    public void GoTo(Vector3 destination)
    {
        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    public void Stop()
    {
        agent.isStopped = true;
    }
    
}

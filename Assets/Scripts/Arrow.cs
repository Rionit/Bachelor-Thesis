/*
 * Author: Filip Doležal
 * Date: 28.4.2025
 * 
 * Description: Handles the size when the arrow is far from the user
 *              It gets progressively smaller
 *              
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameObject camera;

    // Settings
    public float referenceDistance = 1f;
    public float referenceScale = 0.15f;
    public float minScale = 0.0f;

    void Start()
    {
        camera = Camera.main?.gameObject;
    }

    void Update()
    {
        if (camera == null) return;

        float distance = Vector3.Distance(transform.position, camera.transform.position);

        // Scale calculation
        float scaleFactor = referenceDistance / distance;
        scaleFactor = Mathf.Clamp(scaleFactor, minScale, referenceScale);

        transform.localScale = Vector3.one * scaleFactor;
    }
}
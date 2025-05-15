/*
 * Author: Filip Doležal
 * Date: 18.1.2025
 * 
 * Description: Handles particle system when user arrives to destination
 *              
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Finish : MonoBehaviour
{
    public ParticleSystem particles;

    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        if (!particles.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}

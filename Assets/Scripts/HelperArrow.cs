using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperArrow : MonoBehaviour
{
    public Transform target;
    
    private Image arrow;
    private Camera cam;
    
    private void Start()
    {
        arrow = GetComponent<Image>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        bool visible = IsVisible(target.gameObject);
        arrow.enabled = !visible;

        if (!visible)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Vector3 screenPos = cam.WorldToScreenPoint(target.position);

            // It can be behind the camera, so we get rid of that issue
            if (screenPos.z < 0)
            {
                screenPos *= -1;
            }

            Vector3 dir = (screenPos - screenCenter).normalized;

            // TODO: check if this still works for weird ratio phone screens
            float padTop = 650f;
            float padBottom = 650f;
            float padSide = 100f;
            Vector3 edgePos = screenCenter + dir * ((Screen.height / 2f) - padSide); // Push to the edge of screen
            // then clamp it to the "visible area"
            edgePos.x = Mathf.Clamp(edgePos.x, padSide, Screen.width - padSide);
            edgePos.y = Mathf.Clamp(edgePos.y, padTop, Screen.height - padBottom);
            arrow.rectTransform.position = edgePos;

            // Rotate arrow to point toward the target
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrow.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    
    // used from https://github.com/zloedi/offscreen_markers/blob/master/Assets/OffscreenMarker.cs
    private bool IsVisible(GameObject objectToCheck) {
        Plane [] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        Renderer [] rends = objectToCheck.GetComponentsInChildren<Renderer>();
        foreach (var r in rends) {
            if (GeometryUtility.TestPlanesAABB(planes, r.bounds)) {
                return true;
            }
        }
        return false;
    }
}
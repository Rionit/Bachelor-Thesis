/*
 * Author: Filip Doležal
 * Date: 12.10.2024
 * 
 * Description: Calculates time of localization and handles some UI stuff 
 *              
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Immersal.XR;
using Immersal;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ARManager : MonoBehaviour
{

    public TextMeshProUGUI currentLocationLabel;    // Display current location name
    public TextMeshProUGUI elapsedTimeLabel;        // Display elapsed time between successful localizations
    public ImmersalSession session;                 //
    public Localizer localizer;                     // Invokes localization events
    public ImmersalSDK sdk;                         // 
    public Image greenGlow;                         // Green glow of the edge of screen
    public UniversalRendererData urd;
    
    private List<float> _localizationTimes = new List<float>();
    private float _maxLocalizationTime = 0f;
    private float _startTime;   // Time when the first localization started
    private float _elapsedTime; // Time between successful localizations

    public void LocationChanged(GameObject newLocation)
    {
        currentLocationLabel.text = newLocation.name;
    }

    public void OnLocalizationStart()
    {
        _startTime = Time.time;
    }

    public void OnFirstSuccessfulLocalization()
    {
        // _elapsedTime = Time.time - _startTime;
        // elapsedTimeLabel.text = "Čas 1. lokalizace:  " + _elapsedTime.ToString("0.000") + " s";
        // Debug.Log("First successful localization was after: " + _elapsedTime.ToString("0.000") + " s");
    }
    
    public void OnSuccessfulLocalization(int[] mapIds)
    {
        _elapsedTime = Time.time - _startTime;
        _localizationTimes.Add(_elapsedTime);

        // Peak time
        if (_elapsedTime > _maxLocalizationTime)
            _maxLocalizationTime = _elapsedTime;

        float averageTime = _localizationTimes.Average();

        // Display all times
        elapsedTimeLabel.text = 
            $"Δt: {_elapsedTime:0.000} s | " +
            $"avg: {averageTime:0.000} s | " +
            $"max: {_maxLocalizationTime:0.000} s";

        Debug.Log($"Localization: {_elapsedTime:0.000}s | Avg: {averageTime:0.000}s | Max: {_maxLocalizationTime:0.000}s");

        StartCoroutine(Glow());
        _startTime = Time.time;
    }

    public void ToggleSSAO(Toggle toggle)
    {
        urd.rendererFeatures.Where(feature => feature.name == "ScreenSpaceAmbientOcclusion")
            .ToList().ForEach(feature => feature.SetActive(toggle.isOn));
    }

    // Makes the edge of the screen breathe with a green glow
    IEnumerator Glow()
    {
        Color initialColor = greenGlow.color;
    
        float duration = 2f; 
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, time / duration);
            greenGlow.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null; 
        }

        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, time / duration);
            greenGlow.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null; 
        }

        greenGlow.color = initialColor;
    }

    
    // Just for debug purposes
    
    public void OnResetSession()
    {
        session.TriggerResetSession();
    }
    
    public void OnResetLocalizer()
    {
        _ = localizer.StopAndCleanUp();
        session.TriggerResetSession();
    }
    
    public void OnResetSDK()
    {
        sdk.RestartSdk();
    }
}

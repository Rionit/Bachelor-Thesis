/*
 * Author: Filip Doležal
 * Date: 18.1.2025
 * 
 * Description: One element in Category. Changes destination to its Location when clicked.
 *              
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationButton : MonoBehaviour
{
    public TextMeshProUGUI label;

    public void Initialize(string name)
    {
        gameObject.name = name;
        label.text = name;
    }

    public void ChangeDestination(){
        NavigationManager.Instance.DestinationChanged(gameObject.name);
    }
}

/*
 * Author: Filip Doležal
 * Date: 18.1.2025
 * 
 * Description: Category in custom dropdown menu. Populates itself
 *              with Locations that have the same type as this category.
 *              
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Category : MonoBehaviour
{
    public string categoryName;                 // Category name
    public Location.Types type;                 // Type of the locations in this category
    public TextMeshProUGUI header;              // Text showing the name of the category 
    public GameObject body;                     // Here are instances of location buttons
    public NavigationManager navigationManager; // Reference for list of locations
    public GameObject locationButton;           // Location button prefab instance

    void Start()
    {
        if (navigationManager == null) navigationManager = NavigationManager.Instance;
        
        // Get all locations with the right type for this category
        foreach(GameObject location in navigationManager.locations){
            if((location.GetComponent<Location>().type & type) != 0){
                AddLocation(location);
            }
        }
    }

    private void AddLocation(GameObject location){
        LocationButton button = Instantiate(locationButton, body.transform).GetComponent<LocationButton>();
        button.Initialize(location.name);
    }

    // So that in editor it displays automatically the right name
    void OnValidate()
    {
        gameObject.name = categoryName == null ? "Collapsable Window" : categoryName;
        header.text = categoryName;
    }

}

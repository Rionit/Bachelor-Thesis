/*
 * Author: Filip Doležal
 * Date: 25.10.2024
 * 
 * Description: Populates dropdown with locations. Deprecated, was used for simple
 *              dropdown during Semestral project testing.
 *              
 */

using TMPro;
using UnityEngine;

public class DropdownOptionsPopulator : MonoBehaviour
{
    public NavigationManager locManager;
    
    // Deprecated, was used while testing with the old dropdown
    void Start()
    {
        TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
        foreach (GameObject option in locManager.locations)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(option.name, null)); 
        }

        dropdown.captionText.text = "Zde zvolte cílovou destinaci";
    }
}

/*
 * Author: Filip Doležal
 * Date: 4.11.2024
 * 
 * Description: Location. Can be as destination or just a Location
 *              which the user has to go through. Can have multiple
 *              types and has a door when it is a room.
 *              
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Immersal.Samples.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Location : MonoBehaviour
{
    [Flags]
    public enum Types { 
        NONE = 0, 
        PLACE = 1 << 0, 
        ROOM = 1 << 1, 
        CLASSROOM = 1 << 2, 
        LECTURE_ROOM = 1 << 3, 
        WC = 1 << 4, 
        GROUND_FLOOR = 1 << 5, 
        FLOOR_1 = 1 << 6, 
        FLOOR_2 = 1 << 7, 
        FLOOR_3 = 1 << 8,
        FLOOR_4 = 1 << 9,
        STAIRS = 1 << 10
        };

    public string _name;                            // name of the location
    public TextMeshPro label;                       // text shown on door
    public Types type;                              // type(s) of the location
    public UnityEvent<GameObject> locationChange;   // when user enters this location
    public IsNavigationTarget navigationTarget;     // for streamline
    public GameObject door;                         // door prefab
    private bool _hasDoor;                          // if this location has a door
    public bool isDestination                       // if it currently is selected as destination (to hide/show door) 
    {
        get
        {
            return _hasDoor;
        }
        set
        {
            _hasDoor = value;
            door.SetActive(((type & Types.ROOM) != 0 || (type & Types.WC) != 0) && _hasDoor);
        }
    }

    // Automatically set name of the gameobject in hierarchy in editor andd hide/show door
    void OnValidate()
    {
        gameObject.name = _name;
        door.SetActive(((type & Types.ROOM) != 0 || (type & Types.WC) != 0));
        label.text = (type & Types.WC) != 0 ? "WC" : _name;
    }
    
    private void Start()
    { 
        label.text = (type & Types.WC) != 0 ? "WC" : _name;
        isDestination = false;
    }
    
    // User has entered the location
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("MainCamera") || other.CompareTag("EditorOnly")) {
            locationChange.Invoke(gameObject);
        }
    }
}

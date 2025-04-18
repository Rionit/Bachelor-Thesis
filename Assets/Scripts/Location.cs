using System;
using System.Collections;
using System.Collections.Generic;
using Immersal.Samples.Navigation;
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

    public string _name;
    public Types type;
    public UnityEvent<GameObject> locationChange;
    public IsNavigationTarget navigationTarget;
    public GameObject door;
    private bool _hasDoor;
    public bool isDestination
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

    void OnValidate()
    {
        gameObject.name = _name;
        door.SetActive(((type & Types.ROOM) != 0 || (type & Types.WC) != 0));
    }
    
    private void Start()
    { 
        isDestination = false;
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("MainCamera") || other.CompareTag("EditorOnly")) {
            locationChange.Invoke(gameObject);
        }
    }

}

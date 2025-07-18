/*
 * Author: Filip Doležal
 * Date: 25.10.2024
 * 
 * Description: Main script that calculates and updates the path to destination,
 *              handles location change events,
 *              turns visualization of navigation methods on/off
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{
    // Singleton instance
    public static NavigationManager Instance { get; private set; }

    [Header("Setup")]
    //[Tooltip("Dropdown containing all possible destinations and the currently selected one.")]
    // public TMP_Dropdown destinations;
    public GameObject destinationsMenu;     // Menu containing all destinations, is categorized
    public GameObject destinationBtn;       // Button which opens the menu
    public TextMeshProUGUI destinationText; // Text showing the name of the destination
    public TextMeshProUGUI directionsText;  // Text showing directions to next location in path
    public GameObject[] locations;          // All existing locations
    public Edge[] edges;                    // All existing edges
    public GameObject exit;                 // Exit location
    
    public GameObject finishOverlay;        // Prefab that instantiates when user gets to finish
    public GameObject canvas;               

    public RobotController robot;

    [Header("Debug")] 
    public Toggle arrowsToggle;
    public Toggle lineToggle;
    public Toggle robotToggle;
    
    [Header("Immersal Path Navigation")]
    public GameObject testCam;  // Used for testing
    public Streamline lineNear;
    public Streamline lineFar;
    
    private GameObject camera; // user camera
    private List<GameObject> _restrooms = new List<GameObject>();
    private GameObject _location;   // current location
    private GameObject _destination;// current destination
    //private string lastLocation;

    private LinkedList<(Edge edge, bool isForward)> _path; // Tuple to store Edge and direction
    private Dictionary<string, List<Edge>> _adjacencyList; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    private void Start()
    {
        BuildAdjacencyList();
        Hide(edges);

        // Gather all restroom locations into a list
        foreach (GameObject location in locations)
        {
            if ((location.GetComponent<Location>().type & Location.Types.WC) != 0)
            {
                _restrooms.Add(location);
            }
        }

        camera = Camera.main?.gameObject;
#if UNITY_EDITOR
        camera = testCam;
#endif
    }

    private void Update()
    {
        if (_path != null)
        {
            // Update streamlines
            GameObject nextLocation = GetNextLocationInPath();
            if (nextLocation != null && lineToggle.isOn)
            {
                lineNear.ShowFromAToB(camera, nextLocation.GetComponent<Location>().navigationTarget.gameObject);
                lineFar.ShowFromAToB(GetNextLocationInPath().gameObject, _destination.gameObject);
            }
        }
    }
    
    // Dictionary of "What edges am I connected to" for each location
    private void BuildAdjacencyList()
    {
        _adjacencyList = new Dictionary<string, List<Edge>>();
        foreach (Edge edge in edges)
        {
            string nodeA = edge.a.name;
            string nodeB = edge.b.name;

            // Add edge to adjacency list for location A
            if (!_adjacencyList.ContainsKey(nodeA))
            {
                _adjacencyList[nodeA] = new List<Edge>();
            }
            _adjacencyList[nodeA].Add(edge);

            // Add edge to adjacency list for location B
            if (!_adjacencyList.ContainsKey(nodeB))
            {
                _adjacencyList[nodeB] = new List<Edge>();
            }
            _adjacencyList[nodeB].Add(edge);
        }

        Debug.Log("Adjacency list built");
    }

    // Check what toggle changed and update its navigation method accordingly
    public void ToggleChanged(Toggle toggle)
    {
        if (toggle == arrowsToggle && !toggle.isOn) Hide(_path);
        else if (toggle == arrowsToggle && toggle.isOn) ShowPath();
        else if (toggle == lineToggle && !toggle.isOn) HideLines();
        else if (toggle == robotToggle) robot.gameObject.SetActive(toggle.isOn);
    }

    // Hide streamlines
    private void HideLines()
    {
        lineNear.HideNavmeshPath();
        lineFar.HideNavmeshPath();
    }
    
    // When user has come to a new location
    public void LocationChanged(GameObject newLocation)
    {
        //lastLocation = location;
        _location = newLocation;
        Debug.Log("Current location: " + newLocation.name);
        UpdatePath();
        ShowDirections();
    }

    // When user selects a new destination
    public void DestinationChanged(string newDestination)
    {
        // string destName = destinations.options[destinations.value].text;

        if(_destination != null) _destination.GetComponent<Location>().isDestination = false; // disable last
        _destination = locations.FirstOrDefault(x => x.name == newDestination);       // Find the location
        _destination.GetComponent<Location>().isDestination = true;                             // set the new

        destinationText.text = newDestination;  // Update destination text
        destinationBtn.SetActive(true);         // Show destination menu button
        destinationsMenu.SetActive(false);      // Hide destination menu
        
        Debug.Log("Current destination: " + _destination.name);
        
        Hide(_path);
        FindPath();
        ShowPath();
        ShowDirections();
    }

    // Show text directions
    private void ShowDirections()
    {
        if (_path == null)
        {
            directionsText.text = "";
            return;
        } else if(_path.Count == 0)
        {
            directionsText.text = "";
            Instantiate(finishOverlay, gameObject.transform);
            return;
        }

        directionsText.text = First().isForward ? First().edge.fromAToB : First().edge.fromBToA;
    }

    // Multipurpose function to hide edges
    public void Hide(IEnumerable what)
    {
        if (what == null) return;

        foreach (var edge in what)
        {
            if (edge is Edge e) e.Hide();                   // Here edge is just Edge
            else if (edge is (Edge _e, bool _)) _e.Hide();  // Here it is in a tuple
            else return;
        }
    }

    public GameObject GetCurrentLocationInPath()
    {
        if(_path == null || _path.Count == 0) return null;
        return First().isForward ? First().edge.a : First().edge.b;
    }

    public GameObject GetNextLocationInPath()
    {
        if(_path == null || _path.Count == 0) return null;
        return First().isForward ? First().edge.b : First().edge.a;
    }

    // First edge in path, returns a tuple
    private (Edge edge, bool isForward) First()
    {
        return _path.First.Value;
    }

    public bool HasDestination()
    {
        return _destination != null;
    }
    
    public void ChangeDestination(GameObject location)
    {
        // destinations.value = destinations.options.FindIndex(option => option.text == location.name);
        DestinationChanged(location.name);
    }

    // Could be remade so that it just searches using type and BFS
    public void FindNearestRestroom()
    {
        if (_restrooms.Contains(locations.FirstOrDefault(loc => loc == _destination))) return;

        Hide(this._path);
        List<LinkedList<(Edge edge, bool isForward)>> paths = new List<LinkedList<(Edge, bool)>>();
        
        foreach(GameObject wc in _restrooms)
        {
            _destination = wc;
            FindPath();
            paths.Add(this._path);
        }
        
        var path = paths.Aggregate((prev, next) => prev.Count > next.Count ? next : prev);
        var val = path.Last.Value;
        ChangeDestination(val.isForward ? val.edge.b : val.edge.a);
    }

    public void FindExit()
    {
        if(exit == _destination) return;
        
        Hide(_path);
        ChangeDestination(exit);
    }

    // When user gets to new location, it updates the path accordingly
    private void UpdatePath()
    {
        if(_path == null) return;

        // Delete locations user has already been through
        while (_path.Count != 0 && GetCurrentLocationInPath() != _location)
        {
            First().edge.Hide();
            _path.RemoveFirst();
        }

        // User is in a wrong place, find them a new path
        if (_path.Count == 0)
        {
            FindPath();
            ShowPath();
        }
    }

    public void ShowPath()
    {
        if (this._path == null || this._path.Count == 0 || !arrowsToggle.isOn) return;

        //string path = First().isForward ? First().edge.a.name : First().edge.b.name;

        foreach (var (edge, isForward) in this._path)
        {
            if (isForward)
            {
                //path += " -> " + edge.b.name;
                edge.ShowForward();
            }
            else
            {
                //path += " -> " + edge.a.name;
                edge.ShowBackward();
            }
        }

        //Debug.Log(_path);
    }

    private void FindPath()
    {
        if (_location == _destination) return;
        
        _path = new LinkedList<(Edge, bool)>();
        Queue<List<(Edge edge, bool isForward)>> queue = new Queue<List<(Edge, bool)>>();
        HashSet<GameObject> visited = new HashSet<GameObject>();

        // Initialize the BFS queue with paths starting from the current location
        queue.Enqueue(new List<(Edge, bool)>());
        visited.Add(_location);

        while (queue.Count > 0)
        {
            var currentPath = queue.Dequeue();
            GameObject currentNode = _location;

            // Get the last node of the current path
            if (currentPath.Count > 0)
            {
                var lastEdge = currentPath[^1];
                currentNode = lastEdge.isForward ? lastEdge.edge.b : lastEdge.edge.a;
            }

            // Check if we've reached the destination
            if (currentNode == _destination)
            {
                _path = new LinkedList<(Edge, bool)>(currentPath);
                return; // Exit once the shortest path is found
            }

            // Explore neighbors of the current node
            if (_adjacencyList.ContainsKey(currentNode.name))
            {
                foreach (Edge edge in _adjacencyList[currentNode.name])
                {
                    GameObject neighbor;
                    bool isForward;

                    if (edge.a == currentNode)
                    {
                        neighbor = edge.b;
                        isForward = true;
                    }
                    else
                    {
                        neighbor = edge.a;
                        isForward = false;
                    }

                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        // Create a new path extending the current path
                        var newPath = new List<(Edge, bool)>(currentPath)
                        {
                            (edge, isForward)
                        };
                        queue.Enqueue(newPath);
                    }
                }
            }
        }

        // If no path is found, clear the path
        Debug.Log("No path found from " + _location + " to " + _destination);
        _path = null;
    }
}

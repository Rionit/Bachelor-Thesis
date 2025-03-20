using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Edge : MonoBehaviour
{
    [Header("Locations")]
    public GameObject a; 
    public GameObject b;
    [Space(10)]

    [Header("Text Directions")]
    [TextArea] public string fromAToB;
    [TextArea] public string fromBToA;
    [Space(10)]

    [Header("Path of Edge")]
    public GameObject arrow;
    [Range(0, 15)]
    [Tooltip("Number of arrows in one segment of the path.")]
    public int segmentCount;
    [Tooltip("Order sensitive list of points creating the path.")]
    [ContextMenuItem("Add Point", "AddPoint")]
    public List<GameObject> points = new List<GameObject>();

    // Lists for forward and backward arrows
    private List<GameObject> _arrowsForward = new List<GameObject>(); // A -> B
    private List<GameObject> _arrowsBackward = new List<GameObject>();// B -> A

    private Vector3 InterpolatedPointPosition()
    {
        return Vector3.Lerp(a?.transform.position ?? Vector3.zero,
                            b?.transform.position ?? Vector3.zero,
                            (float)points.Count / (float)segmentCount);
    }

    // Used to add a point for arrow from editor
    public void AddPoint()
    {
        GameObject point = new GameObject("p" + points.Count);
        point.transform.parent = transform;
        point.transform.position = InterpolatedPointPosition();
        points.Add(point);
    }

    private void Start()
    {
        GenerateArrows();
        Hide();
    }

    private void GenerateArrows()
    {
        // Generate forward arrows (a -> b)
        for (int i = 0; i < points.Count - 1; i++)
        {
            GameObject arrowForward = Instantiate(arrow, transform);
            Vector3 start = points[i].transform.position;
            Vector3 end = points[i + 1].transform.position;

            Vector3 direction = end - start;
            Quaternion rotation = Quaternion.LookRotation(-direction, Vector3.up);

            arrowForward.transform.position = Vector3.Lerp(start, end, 0.5f);
            arrowForward.transform.rotation = rotation;
            _arrowsForward.Add(arrowForward);
        }

        // Generate backward arrows (b -> a)
        for (int i = points.Count - 1; i > 0; i--)
        {
            GameObject arrowBackward = Instantiate(arrow, transform);
            Vector3 start = points[i].transform.position;
            Vector3 end = points[i - 1].transform.position;

            Vector3 direction = end - start;
            Quaternion rotation = Quaternion.LookRotation(-direction, Vector3.up);

            arrowBackward.transform.position = Vector3.Lerp(start, end, 0.5f);
            arrowBackward.transform.rotation = rotation;
            _arrowsBackward.Add(arrowBackward);
        }
    }

    public void ShowForward()
    {
        foreach (GameObject arrow in _arrowsForward)
        {
            arrow.SetActive(true);
        }
        
        b.GetComponent<Location>().navigationTarget.gameObject.SetActive(true);
    }

    public void ShowBackward()
    {
        foreach (GameObject arrow in _arrowsBackward)
        {
            arrow.SetActive(true);
        }
        
        a.GetComponent<Location>().navigationTarget.gameObject.SetActive(true);
    }

    public void Hide()
    {
        foreach (GameObject arrow in _arrowsForward)
        {
            arrow.SetActive(false);
        }
        foreach (GameObject arrow in _arrowsBackward)
        {
            arrow.SetActive(false);
        }
        
        a.GetComponent<Location>().navigationTarget.gameObject.SetActive(false);
        b.GetComponent<Location>().navigationTarget.gameObject.SetActive(false);
    }

    // name of edge changes in Inspector automatically based on locations a and b
    void OnValidate()
    {
        gameObject.name = a.name + " <-> " + b.name;
    }
}

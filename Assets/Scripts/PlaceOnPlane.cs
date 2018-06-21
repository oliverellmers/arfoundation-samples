using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSessionOrigin))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject[] m_PlacedPrefab;


    private bool IsFingerHeldDown = false;

    public void FingerDoubleTap() {
        Debug.Log("Double tap! Destroy object");
        Destroy(placedObject);
        placedObject = null;
    }

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab[prefabIndex]; }
        set { m_PlacedPrefab[prefabIndex] = value; }
    }

    bool b = false;
    int prefabIndex = 0;
    public void SwapPlacedPrefab() {

        Debug.Log("Swiped! Changed the prefab...");

        b = !b;

        if (b)
        {
            prefabIndex = 0;
        }
        else {
            prefabIndex = 1;
        }
    }



    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject placedObject { get; private set; }

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    void Update()
    {
        if (Input.touchCount == 2)
            return;

        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);

        var hits = s_RaycastHits;
        
        if (!m_SessionOrigin.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            return;

        placementHit = hits[0];
    }

    ARSessionOrigin m_SessionOrigin;

    ARRaycastHit m_PlacementHit;

    ARRaycastHit placementHit
    {
        get { return m_PlacementHit; }
        set
        {
            m_PlacementHit = value;

            if (placedObject == null && m_PlacedPrefab[prefabIndex] != null) //Here is where we are initially instantiated
            {
                //Here
                placedObject = Instantiate(m_PlacedPrefab[prefabIndex]);
            }

            if (placedObject != null)
            {
                var pose = m_PlacementHit.pose;
                placedObject.transform.position = pose.position;
                placedObject.transform.rotation = pose.rotation;
            }
        }
    }

    List<ARRaycastHit> s_RaycastHits = new List<ARRaycastHit>();
}

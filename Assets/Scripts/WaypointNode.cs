using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to create the characteristics of the Waypoint Node for
/// Timmy's Pathfinding Algorithm (A*).
/// 
/// The Node has the following properties:
///     nodeList: List of nodes connected to it
///     G: Path cost between nodes
///     H: Heuristic value(s) (Which Timmy will calculate) (TBD)
///     F: G + H (TBD)
///     
/// 
/// </summary>
public class WaypointNode : MonoBehaviour
{

    [SerializeField] GameObject parent;

    // [SerializeField] private bool isTriggered;
    [SerializeField] private Pathfinding pf;

    //[SerializeField] private int numWalls = 0;

    // parent node
    private WaypointNode parentNode;

    /// <summary>
    /// Stores the parent node of this node. Useful in reverse reconstruction.
    /// </summary>
    public WaypointNode ParentNode
    {
        get
        {
            return parentNode;
        }
        set
        {
            parentNode = value;
        }
    }

    /**
     * Store only immediate neighbors for quicker look up in PF
     */
    private Dictionary<GameObject, float[]> _neighbors;

    /// <summary>
    /// Access neighbors as a C# Property. Read-only outside of this
    /// class.
    /// </summary>
    public Dictionary<GameObject, float[]> Neighbors
    {
        get
        {
            return _neighbors;
        }
        private set
        {
            _neighbors = value;
        }
    }


    /**
     * Store all non-neighbor nodes with their g and h values as a Dictionary
     */
    private Dictionary<GameObject, float[]> _nodeMap;

    /// <summary>
    /// Access the NodeMap as a C# Property. Read-only outside of this
    /// class.
    /// </summary>
    public Dictionary<GameObject, float[]> NodeMap
    {
        get
        {
            return _nodeMap;
        }
        private set
        {
            _nodeMap = value;
        }
    }

    
    private void Awake()
    {
        NodeMap = new Dictionary<GameObject, float[]>();
        ConnectNodes();
        Debug.Log($"Current Node: {this.gameObject.name}");
        foreach (GameObject node in NodeMap.Keys)
        {
            Debug.Log($"Waypoint: {node.name}\nG Value: {NodeMap[node]}");
        }
    }

    /// <summary>
    /// For each node, I want to raycast to another waypoint in
    /// the maze, and if there is no collider/wall blocking the
    /// raycast, add it to the NodeList.
    /// </summary>
    private void ConnectNodes()
    {
        //Transform parent = GetComponentInParent<GameObject>().transform;
        foreach (Transform child in parent.transform)
        {
            
            if (!child.gameObject.Equals(this.gameObject))
            {

                float g = (child.position - transform.position).magnitude;
                // Get the array of everything hit with the RaycastAll
                RaycastHit[] raycastHits = Physics.RaycastAll(origin: this.gameObject.transform.position,
                    direction: child.position - this.gameObject.transform.position,
                    maxDistance: g);

                // If there's only one thing in the array, check if it's a waypoint
                if (raycastHits.Length == 1 && raycastHits[0].collider.gameObject.CompareTag("Waypoint"))
                {
                    // If it is a waypoint, draw a green line for debugging purposes.
                    // g = (child.position - this.gameObject.transform.position).magnitude;
                    Debug.DrawRay(this.gameObject.transform.position,
                        child.position - this.gameObject.transform.position,
                        color: Color.green,
                        duration: Mathf.Infinity);
                    
                    // add the neighbor to the neighbors dict
                    Neighbors.Add(child.gameObject, new float[2] { g, 0 });
                }
                else
                {
                    // Otherwise, loop through the array and count the number of walls
                    /*foreach (RaycastHit hitItem in raycastHits)
                    {
                        Debug.Log($"Raycast from {this.gameObject.name} hit {hitItem.collider.gameObject.name}");
                        if (!hitItem.transform.gameObject.CompareTag("Waypoint"))
                        {
                            h++; 
                        }
                        else
                        {
                            g = (child.position - this.gameObject.transform.position).magnitude;
                        }
                    }*/

                    // -1 because one of the hits is the waypoint itself (assuming the waypoints count as a hit)
                    float h = raycastHits.Length() - 1;
                    // following line moved into the else statement so that immediate neighbors don't get double counted in the NodeMap
                    NodeMap.Add(child.gameObject, new float[2] { g, h });
                }
                // NodeMap.Add(child.gameObject, new float[2] { g, h });
            }
        }

    }

    /// <summary>
    /// Activated when a collider enters the trigger.
    /// </summary>
    /// <param name="other">Collider that enters the trigger.</param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            Debug.Log($"{collider.gameObject.name} has collided with {this.gameObject.name}");
            pf.SetGoal(this);
            //isTriggered = true;
        }
    }

   /* /// <summary>
    /// Activated when a collider stays in the trigger and does not leave.
    /// </summary>
    /// <param name="collider">Collider that stays in the trigger.</param>
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            isTriggered = true;
        }
    }*/

    /// <summary>
    /// Activated when a collider leaves the trigger.
    /// </summary>
    /// <param name="collider">Collider that leaves the trigger.</param>
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            Debug.Log($"{collider.gameObject.name} has exited from collider {this.gameObject.name}");
            //parent.GetComponent<>().SetGoal(this);
            //isTriggered = false;

            // TO DO: may need to add a clear path for PF line in here depending on whether other waypoints
            //        successfully overwrite the goal as needed... probably should in case out of bounds?
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    //DidRaycastHitPlayer();
    //}
}

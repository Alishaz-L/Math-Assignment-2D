using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject childrenPrefab;
    public Transform spawnPoint;
    public int childrenCount = 10;
    public float force = 5f;
    public LayerMask childMask;

    private List<GameObject> children = new List<GameObject>();
    private LineRenderer lineRenderer;

    void Start()
    {
        for (int i = 0; i < childrenCount; i++)
        {
            GameObject child = Instantiate(childrenPrefab, spawnPoint.position, Quaternion.identity);
            child.SetActive(false);
            children.Add(child);
        }

        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shootChildren();
        }
    }

    void shootChildren()
    {
        // Clear previous line renderer data
        lineRenderer.positionCount = 0;

        // Add the player's position to the line renderer
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(0, spawnPoint.position);

        // Get all children that the player is colliding with
        List<GameObject> collidedChildren = new List<GameObject>();
        foreach (GameObject child in children)
        {
            if (child.activeSelf && child.GetComponent<BoxCollider>().bounds.Intersects(GetComponent<BoxCollider>().bounds));
            {
                collidedChildren.Add(child);
            }
        }

        // Create a line between the player and the collided children
        foreach (GameObject child in collidedChildren)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, child.transform.position);
        }

        // Add force to the collided children in the direction of the line
        foreach (GameObject child in collidedChildren)
        {
            Vector3 forceDirection = (child.transform.position - spawnPoint.position).normalized;
            child.GetComponent<Rigidbody>().AddForce(forceDirection * force, ForceMode.Impulse);
        }
    }
}
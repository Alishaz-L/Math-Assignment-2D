using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POWERUP : MonoBehaviour
{
    /*****************************/
    /* these are shown in the inspector in unity */
    /* this lets you have a dropdown in the inspector */
    public enum PowerupType
    {
        TRANSLATE = 0,
        ROTATE,
        SCALE
    }

    // new state variable for rotation
    bool isRotating = false;

    // new state variable for scaling
    bool isScaling = false;
    bool Scaled = false;
    Vector3 Smallest = new Vector3(0.15f, 0.15f, 0.15f);
    Vector3 originalScale = new Vector3(0.3f, 0.3f, 0.3f);

    [SerializeField]
    Transform Position;

    [SerializeField]
    public GameObject Human;

    [SerializeField]
    public PowerupType PType;

    [SerializeField]
    float Speed = 10f;

    [SerializeField]
    Vector3 deltaVector;

    [SerializeField]
    Vector3 offset; // offset for children object positions
    /*****************************/

    bool isON = false; // is this powerup active?

    /*****************************/
    /* utility functions you may need or want */
    /* utility function to get a list of children of a given game object */
    public List<GameObject> GetChildren(GameObject obj)
    {
        Debug.Log("GetChildren");
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in obj.transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }

    /* utility function to get a list of ALL children of a given game object */
    public List<GameObject> GetAllChildren(GameObject obj)
    {
        List<GameObject> children = GetChildren(obj);
        for (var i = 0; i < children.Count; i++)
        {
            List<GameObject> moreChildren = GetChildren(children[i]);
            for (var j = 0; j < moreChildren.Count; j++)
            {
                children.Add(moreChildren[j]);
            }
        }
        return children;
    }

    /* utility function to get a list of ALL children of a given game object with a particular NAME */
    public List<GameObject> FindChildrenWithName(string nam, GameObject obj)
    {
        List<GameObject> children = GetAllChildren(obj);
        List<GameObject> results = new List<GameObject>();
        for (var i = 0; i < children.Count; i++)
        {
            if (children[i].name == nam)
                results.Add(children[i].gameObject);
        }
        return results;
    }
    /*****************************/

    /* feel free to use/modify/change all of these */
    /* there are more than one way to solve this problem */
    void applyPowerUp(POWERUP p)
    {
        if (p)
        {
            switch (p.PType)
            {
                case PowerupType.TRANSLATE:
                    TranslatePower(p);
                    break;
                case PowerupType.ROTATE:
                    RotatePower(p);
                    break;
                case PowerupType.SCALE:
                    ScalePower(p);
                    break;
            }
        }
    }

    void TranslatePower(POWERUP p)
    {
        if (p.Human != null)
        {
            // Calculate the translation amount based on speed and the right direction
            Vector3 translation = Vector3.right * p.Speed * Time.fixedDeltaTime;

            // Apply translation to the game object
            p.transform.Translate(translation);
        }
    }

    void RotatePower(POWERUP p)
    {
        if (p.Human != null)
        {
            // Check if rotation is active
            if (isRotating)
            {
                // Rotate the object clockwise
                p.transform.Rotate(Vector3.forward * p.Speed * Time.fixedDeltaTime);
            }
        }
    }

    void ScalePower(POWERUP p)
    {
        if (p.Human != null)
        {
            if (p.isScaling)
            {
                if (!p.Scaled)
                {
                    // Shrink the object to half its size
                    p.transform.localScale = Vector3.Lerp(p.transform.localScale, p.Smallest, 0.1f);

                    // Check if the scale is close to the smallest scale
                    if (Vector3.Distance(p.transform.localScale, p.Smallest) < 0.01f)
                    {
                        p.Scaled = true;
                    }
                }
                else
                {
                    // Return to the original size
                    p.transform.localScale = Vector3.Lerp(p.transform.localScale, p.originalScale, 0.1f);

                    // Check if the scale is close to the original scale
                    if (Vector3.Distance(p.transform.localScale, p.originalScale) < 0.01f)
                    {
                        p.Scaled = false;
                    }
                }
            }
        }
    }




    // Update is called once per frame
    void FixedUpdate()
    {
        if (isON)
        {
            // apply powerup to the object this script is attached to
            // you may wish to do this elsewhere
            applyPowerUp(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            
            // player triggered this
            isON = !isON;

            // Toggle rotation state on each collision
            isRotating = !isRotating;

            // Toggle scaling down state on each collision
            isScaling = !isScaling;

            // make a small thumbnail here and add as a child to the player
            // make a clone of ourselves
            var obj = Instantiate(this, Position );

            // change the name of the object, you may wish to use something different 
            // to denote the different powerups 
            string childname = "CHILD";
            obj.name = childname;

            // remove all powerup component scripts from the clone 
            // otherwise you will have an infinite loop and it will crash your PC
            Destroy(obj.GetComponent<POWERUP>());

            // how many children are already attached to the player?
            // you may wish to use a specific powerup name to see how many powerups are already applied
            // hint: think of only having 1 type of powerup shown, maybe you need to do something here or before
            int numChildren = FindChildrenWithName(childname,Human).Count;

            // set the position of the child based on how many already exist
            obj.transform.localPosition = offset * numChildren;

            // set the scale to be small
            obj.transform.localScale /= 5f;

        }
        else if (collision.gameObject.name == "ShortBuilding")
        {
            // Reverse the direction when colliding with ShortBuilding
            Speed = -Speed;

            // Bounce back the power-up object
            Vector3 bounceDirection = -transform.right; // Use -transform.right for the opposite direction
            transform.Translate(bounceDirection * Speed * Time.fixedDeltaTime);
        }
    }
}


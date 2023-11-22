using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Rigidbody2D Licorice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Rigidbody2D rb = Instantiate(Licorice, Player.transform.position + new Vector3(0.5f,0,0), transform.rotation);
            rb.AddForce(10*Vector3.right, ForceMode2D.Impulse);
            Destroy(Licorice, 3f);
        }
    }
}

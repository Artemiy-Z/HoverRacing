using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COM_Setter : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Transform com;
    
    // Start is called before the first frame update
    void Start()
    {
        rb.centerOfMass = com.position - rb.position;
    }
}

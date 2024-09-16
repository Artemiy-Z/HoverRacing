using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMotion : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float sideForce, forwardForce, turningTorque;
    private Vector2 input;

    [SerializeField]
    private LayerMask ground;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!rb)
            enabled = false;
    }

    void Update() {
        input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));

        if(Input.GetKeyDown(KeyCode.R))
        {
            rb.MovePosition(rb.position + Vector3.up * 2);
            rb.MoveRotation(Quaternion.identity);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float coef = 0f;
        Vector3 direction = transform.forward;
        Vector3 upward = transform.up;

        if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, Mathf.Infinity, ground))
        {
            if(hit.distance < 4)
            {
                coef = 1f;

                upward = hit.normal;

                Vector3 projected = Vector3.ProjectOnPlane(direction, hit.normal);
                direction = projected.normalized;
            }
        }

        rb.AddForce(direction * forwardForce * input.y * coef);
        rb.AddRelativeForce(Vector3.right * sideForce * input.x * coef);
        
        if(input.y > 0)
            rb.AddTorque(new Vector3(0, turningTorque * input.x * coef, 0));
        else if(input.y < 0)
            rb.AddTorque(new Vector3(0, -turningTorque * input.x * coef, 0));
        else
        {
            float subInput = 0;

            if (Input.GetKey(KeyCode.E))
                subInput = 1;
            else if (Input.GetKey(KeyCode.Q))
                subInput = -1;

            rb.AddTorque(new Vector3(0, turningTorque * subInput * coef, 0));
        }
    }
}

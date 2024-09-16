using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverCraftBehavior : MonoBehaviour
{
    [SerializeField]
    private float getupTorque = 1f;

    [SerializeField]
    private Transform pointFL, pointFR, pointBL, pointBR;
    
    [SerializeField]
    private Rigidbody rb;
    
    [SerializeField]
    private float forceAmount = 1f;
    [SerializeField]
    private float maxDistance = 1f;
    [SerializeField]
    private LayerMask ground;
    
    private Vector3 rayDirection = Vector3.down;

    [SerializeField]
    private HoverSound hoverSound;

    void Start()
    {
        if(!pointFL || !pointFR || !pointBL || !pointBR)
            enabled = false; // check for all 4 points
        
        if(!rb)
            enabled = false; // check for body
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rayDirection = -transform.up;

        float currentForce = forceAmount * rb.mass;

        float averageDistance = 0f;
        int count = 0;

        Vector3 averageNormal = Vector3.up;
        float fl = 0, fr = 0, bl = 0, br = 0f;
        Vector3 pfl = pointFL.position,
                pfr = pointFR.position,
                pbl = pointBL.position,
                pbr = pointBR.position;
        
        // cast rays down
        if(Physics.Raycast(pointFL.position, rayDirection, out RaycastHit hit, Mathf.Infinity, ground)) {
            fl = Mathf.Clamp(maxDistance / hit.distance, 0, 1);  
            averageDistance += hit.distance;
            if(hit.distance <= 4f)
                count++;

            averageNormal = Vector3.Lerp(averageNormal, hit.normal, 0.5f);
        }
        
        if(Physics.Raycast(pointFR.position, rayDirection, out hit, Mathf.Infinity, ground))
        {
            fr = Mathf.Clamp(maxDistance / hit.distance, 0, 1);
            averageDistance += hit.distance;
            if(hit.distance <= 4f)
                count++;

            averageNormal = Vector3.Lerp(averageNormal, hit.normal, 0.5f);
        }
        
        if(Physics.Raycast(pointBL.position, rayDirection, out hit, Mathf.Infinity, ground))
        {
            bl = Mathf.Clamp(maxDistance / hit.distance, 0, 1);
            averageDistance += hit.distance;
            if(hit.distance <= 4f)
                count++;

            averageNormal = Vector3.Lerp(averageNormal, hit.normal, 0.5f);
        }
        
        if(Physics.Raycast(pointBR.position, rayDirection, out hit, Mathf.Infinity, ground))
        {
            br = Mathf.Clamp(maxDistance / hit.distance, 0, 1);
            averageDistance += hit.distance;
            if(hit.distance <= 4f)
                count++;

            averageNormal = Vector3.Lerp(averageNormal, hit.normal, 0.5f);
        }

        rb.AddForceAtPosition(averageNormal * currentForce * fl, pfl);
        rb.AddForceAtPosition(averageNormal * currentForce * fr, pfr);
        rb.AddForceAtPosition(averageNormal * currentForce * bl, pbl);
        rb.AddForceAtPosition(averageNormal * currentForce * br, pbr);
        
        // aerodynamics
        rb.AddForceAtPosition(-averageNormal * currentForce * fl * rb.velocity.magnitude * 0.01f, pointFL.position);
        rb.AddForceAtPosition(-averageNormal * currentForce * fr * rb.velocity.magnitude * 0.01f, pointFR.position);
        rb.AddForceAtPosition(-averageNormal * currentForce * bl * rb.velocity.magnitude * 0.01f, pointBL.position);
        rb.AddForceAtPosition(-averageNormal * currentForce * br * rb.velocity.magnitude * 0.01f, pointBR.position);

        if (count <= 2)
        {
            rb.AddRelativeTorque(new Vector3(0, 0, -rb.rotation.z) * getupTorque);
        }

        if (count != 0)
        {
            averageDistance /= count;
            if (hoverSound)
                hoverSound.UpdateDistance(averageDistance, maxDistance);
        }
    }
}

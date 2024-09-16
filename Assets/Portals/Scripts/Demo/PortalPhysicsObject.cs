using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PortalPhysicsObject : PortalTraveller {

    public float force = 10;
    new Rigidbody rigidbody;
    public Color[] colors;
    static int i;
    public HoverCraftBehavior hover;

    void Awake () {
        rigidbody = GetComponent<Rigidbody> ();
        hover = GetComponentInChildren<HoverCraftBehavior> ();
        
        i++;
        if (i > colors.Length - 1) {
            i = 0;
        }
    }

    public override void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        base.Teleport(fromPortal, toPortal, pos, rot);

        if (hover)
        {
            hover.enabled = false;
            Invoke("enableH", 0.5f);
        }

        rigidbody.velocity = toPortal.TransformVector (fromPortal.InverseTransformVector (rigidbody.velocity));
    }

    void enableH()
    {
        hover.enabled = true;
    }
}
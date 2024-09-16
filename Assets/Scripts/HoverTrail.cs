using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTrail : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float multiplier = 0.3f;
    [SerializeField]
    private TrailRenderer trail;

    private float originalWidth;
    private float targetWidthLerp = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if(!trail || !rb)
            enabled = false;

        originalWidth = trail.widthMultiplier;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb.velocity.magnitude > 5f)
        {
            targetWidthLerp = rb.velocity.magnitude * multiplier;
            trail.emitting = true;
        }
        else
        {
            targetWidthLerp = Mathf.MoveTowards(targetWidthLerp, 0, Time.fixedDeltaTime * 0.5f);
            trail.emitting = targetWidthLerp < 0;
        }

        targetWidthLerp = Mathf.Clamp(targetWidthLerp, 0, 1);
        trail.widthMultiplier = Mathf.Lerp(0, originalWidth, targetWidthLerp);
    }
}

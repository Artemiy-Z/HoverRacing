using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource clip;
    [SerializeField]
    private float maxPitch = 1.0f;
    [SerializeField]
    private float minPitch = 0.0f;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float velocityPitchMultiplier = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        if(!clip)
            enabled = false;

        clip.loop = true;
        clip.Play();
    }

    public void UpdateDistance(float distance, float maxDistance)
    {
        if(maxPitch < minPitch)
            maxPitch = minPitch;

        float coeffitient = Mathf.Clamp(maxDistance / distance, 0, 1);
        coeffitient = Mathf.Sin(coeffitient * Mathf.PI * 0.5f);

        clip.pitch = minPitch + coeffitient * (maxPitch - minPitch);

        if(rb != null)
        {
            clip.pitch += rb.velocity.magnitude * velocityPitchMultiplier * (Input.GetAxisRaw("Vertical") != 0 ? 1 : 0);
        }
    }
}

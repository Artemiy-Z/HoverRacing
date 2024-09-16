using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBehaviour : MonoBehaviour
{
    private GameObject[] list = new GameObject[3];
    private int previousSide = -2;
    [SerializeField]
    private GameObject readyEffect;
    [SerializeField]
    private ParticleSystem passEffect;
    [SerializeField]
    private AudioSource passSound;
    [SerializeField]
    private CheckPointSystem system;

    private void Start()
    {
        system = transform.parent.GetComponent<CheckPointSystem>();
    }

    private void LateUpdate()
    {
        if (list[0])
        {
            readyEffect.SetActive(true);

            float temp = Vector3.Dot(transform.forward, transform.InverseTransformPoint(list[0].transform.position));
            int currentSide = (temp >= 0 ? 1 : -1);

            if(currentSide == 1 && previousSide == -1)
            {
                print("Player passed");
                list[0] = null;

                passSound.Play();
                passEffect.Play();
                Invoke("deactivate", 0.6f);
            }

            previousSide = currentSide;
        }
        else
        {
            readyEffect.SetActive(false);
        }
    }

    private void deactivate()
    {
        system.UpdateIndex();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            list[0] = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            list[0] = null;
        }
    }
}

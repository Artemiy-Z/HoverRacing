using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject[] checkPoints;
    [SerializeField]
    private int currentIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        checkPoints = new GameObject[transform.childCount];

        int i = 0;
        foreach(Transform child in transform.Cast<Transform>().OrderBy(t=>t.GetSiblingIndex()))
        {
            checkPoints[i] = child.gameObject;
            checkPoints[i].SetActive(false);
            i++;
        }

        UpdateIndex();
    }

    public void UpdateIndex()
    {
        if(currentIndex == -1)
        {
            checkPoints[0].SetActive(true);
            currentIndex = 0;
            return;
        }

        if(currentIndex == checkPoints.Length-1)
        {
            checkPoints[currentIndex].SetActive(false);
            return;
        }

        checkPoints[currentIndex].SetActive(false);
        checkPoints[currentIndex+1].SetActive(true);
        currentIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

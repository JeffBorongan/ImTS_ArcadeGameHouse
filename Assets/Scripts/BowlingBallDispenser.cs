using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBallDispenser : MonoBehaviour
{
    [SerializeField] private GameObject items = null;

    private string objectTag = "";

    private void Start()
    {
        Instantiate(items, transform.position, transform.rotation);
        objectTag = items.tag;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == objectTag)
        {
            Instantiate(items, transform.position, transform.rotation);
        }
    }
}

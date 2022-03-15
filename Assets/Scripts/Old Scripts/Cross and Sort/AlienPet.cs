using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienPet : MonoBehaviour
{
    public GameObject indicator;
    public List<Material> indicatorColor;

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "AlienPet1")
        {
            if (other.gameObject.tag == "AlienFood1")
            {
                indicator.GetComponent<MeshRenderer>().material = indicatorColor[2];
                StartCoroutine(HungerCour(5f));
            } 

            else
            {
                indicator.GetComponent<MeshRenderer>().material = indicatorColor[0];
                StartCoroutine(HungerCour(5f));
            }
        }

        if (gameObject.tag == "AlienPet2")
        {
            if (other.gameObject.tag == "AlienFood2")
            {
                indicator.GetComponent<MeshRenderer>().material = indicatorColor[2];
                StartCoroutine(HungerCour(5f));
            }

            else
            {
                indicator.GetComponent<MeshRenderer>().material = indicatorColor[0];
                StartCoroutine(HungerCour(5f));
            }
        }

        if (gameObject.tag == "AlienPet3")
        {
            if (other.gameObject.tag == "AlienFood3")
            {
                indicator.GetComponent<MeshRenderer>().material = indicatorColor[2];
                StartCoroutine(HungerCour(5f));
            }

            else
            {
                indicator.GetComponent<MeshRenderer>().material = indicatorColor[0];
                StartCoroutine(HungerCour(5f));
            }
        }
    }

    IEnumerator HungerCour(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        indicator.GetComponent<MeshRenderer>().material = indicatorColor[1];
    }
}

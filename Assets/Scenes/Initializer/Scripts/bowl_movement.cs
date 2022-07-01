using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowl_movement : MonoBehaviour
{
    public Transform bowl_status;
    Vector3 reset_position;
    // Start is called before the first frame update
    void Start()
    {
        bowl_status = this.gameObject.transform;
        reset_position = new Vector3(1f, 0.85f, 1.7f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bowl_position = bowl_status.position;

        if(bowl_position.y <= 0.16f)
            StartCoroutine("resetBowl");
    }

    IEnumerator resetBowl(){

        yield return new WaitForSeconds(3);
        
        bowl_status.position = reset_position;

    }
}

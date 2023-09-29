using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pins_movement : MonoBehaviour
{
    public Transform pin_status;
    // Start is called before the first frame update
    void Start()
    {
        pin_status = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pin_rotation = pin_status.eulerAngles;

        //Debug.Log(pin_rotation);

        if(pin_rotation.z >= 80f || pin_rotation.z <= -80f)
            StartCoroutine(ByePins(2f));
        else if(pin_rotation.y >= 80f || pin_rotation.y <= -80f)
            StartCoroutine(ByePins(2f));

        else if(pin_rotation.x >= 80f || pin_rotation.x <= -80f)
            StartCoroutine(ByePins(2f));
    }

    IEnumerator ByePins(float waitTime){
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }
}

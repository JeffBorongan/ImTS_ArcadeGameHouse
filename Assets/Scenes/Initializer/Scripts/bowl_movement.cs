using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowl_movement : MonoBehaviour
{
    public Transform bowl_status;
    public GameObject new_bowl;
    public Vector3 reset_position;
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

        if(bowl_position.y <= 0.16f){
            Debug.Log(bowl_position);
            StartCoroutine(resetBowl());
        }
    }

    IEnumerator resetBowl(){

        yield return new WaitForSeconds(3);
        Instantiate(new_bowl, new_bowl.transform.position, new_bowl.transform.rotation);
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game2_doors : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public GameObject door4;
    public GameObject door5;
    public GameObject signal1;
    public GameObject signal2;
    public GameObject signal3;
    public GameObject signal4;
    public GameObject signal5;
    public Material red_light;
    public Material green_light;
    public int door_no;
    public GameObject left_lever;
    public GameObject right_lever;
    public bool door_lock;


    // Start is called before the first frame update
    void Start()
    {
        door_no = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 left_pos = left_lever.transform.position;
        Vector3 right_pos = right_lever.transform.position;

        if(left_pos.y > 1f && right_pos.y > 1f){

            StartCoroutine(doorUP());
            
        }else if(left_pos.y < 0.3f && right_pos.y < 0.3f){
            door_lock = false;
        }

    }

    public void lift_door(){

        
    }

    IEnumerator doorUP()
    {

        

        yield return new WaitForSeconds(0.1f);

        if(door_no == 1 && door_lock == false){
            
            door_lock = true;
            float initial_pos = door1.transform.position.y;

            Vector3 door_pos = door1.transform.position;

            while(door_pos.y <= (initial_pos + 1f)){
                door_pos.y = door_pos.y + (0.0001f * Time.deltaTime);
            
                door1.transform.position = door_pos;
            }
                
            if(door1.transform.position.y >= 1f){
                door_no = 2;
                signal1.GetComponent<Renderer>().material = green_light;
            }
        }else if(door_no == 2 && door_lock == false){
            
            door_lock = true;
            float initial_pos = door2.transform.position.y;

            Vector3 door_pos = door2.transform.position;

            while(door_pos.y <= (initial_pos + 1f)){
                door_pos.y = door_pos.y + (0.0001f * Time.deltaTime);
            
                door2.transform.position = door_pos;
            }
                
            if(door2.transform.position.y >= 1f){
                door_no = 3;
                signal2.GetComponent<Renderer>().material = green_light;
            }
        }else if(door_no == 3 && door_lock == false){
            
            door_lock = true;
            float initial_pos = door3.transform.position.y;

            Vector3 door_pos = door3.transform.position;

            while(door_pos.y <= (initial_pos + 1f)){
                door_pos.y = door_pos.y + (0.0001f * Time.deltaTime);
            
                door3.transform.position = door_pos;
            }
                
            if(door3.transform.position.y >= 1f){
                door_no = 4;
                signal3.GetComponent<Renderer>().material = green_light;
            }
        }else if(door_no == 4 && door_lock == false){
            
            door_lock = true;
            float initial_pos = door4.transform.position.y;

            Vector3 door_pos = door4.transform.position;

            while(door_pos.y <= (initial_pos + 1f)){
                door_pos.y = door_pos.y + (0.0001f * Time.deltaTime);
            
                door4.transform.position = door_pos;
            }
                
            if(door4.transform.position.y >= 1f){
                door_no = 5;
                signal4.GetComponent<Renderer>().material = green_light;
            }
        }else if(door_no == 5 && door_lock == false){
            
            door_lock = true;
            float initial_pos = door5.transform.position.y;

            Vector3 door_pos = door5.transform.position;

            while(door_pos.y <= (initial_pos + 1f)){
                door_pos.y = door_pos.y + (0.0001f * Time.deltaTime);
            
                door5.transform.position = door_pos;
            }
                
            if(door5.transform.position.y >= 1f){
                door_no = 6;
                signal5.GetComponent<Renderer>().material = green_light;
            }
        }
    }
}

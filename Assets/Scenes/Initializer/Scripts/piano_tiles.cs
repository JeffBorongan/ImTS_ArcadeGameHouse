using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piano_tiles : MonoBehaviour
{
    public Material prev_material;
    public Material hover_material;
    public AudioClip piano_key;
    public AudioSource key_sound;
    // Start is called before the first frame update
    void Start()
    {
        prev_material = this.gameObject.GetComponent<MeshRenderer> ().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void keyPressIn(){
        this.gameObject.GetComponent<MeshRenderer> ().material = hover_material;
        Debug.Log("key pressed");
    }

    public void keyPressOut(){
        this.gameObject.GetComponent<MeshRenderer> ().material = prev_material;
    }

    public void keySoundOn(){
        key_sound.PlayOneShot(piano_key);
    }
}

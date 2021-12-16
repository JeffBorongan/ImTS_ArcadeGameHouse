using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField] protected DoorParameters parameters = null;

    public string RoomName { get => parameters.roomName; }
    public EnvironmentPoints Point { get => parameters.point; }
    public EnvironmentPoints DestinationPoint { get => parameters.destinationPoint; }

    private void Start()
    {
        //Environment.Instance.AddDoor(this);
    }

    public virtual void EnterRoom(UnityAction OnMid, UnityAction OnEnd) {  }

    public virtual void OpenDoor(bool open) 
    {
        if (parameters.leftDoor == null && parameters.rightDoor == null) { return; }

        parameters.doorCollider.enabled = !open;

        if (open)
        {
            parameters.leftDoor.DOLocalMoveX(parameters.leftDoor.transform.localPosition.x + 2.67f, 1f);
            parameters.rightDoor.DOLocalMoveX(parameters.rightDoor.transform.localPosition.x - 2.67f, 1f);
        }
        else
        {
            parameters.leftDoor.DOLocalMoveX(parameters.leftDoor.transform.localPosition.x - 2.67f, 1f);
            parameters.rightDoor.DOLocalMoveX(parameters.rightDoor.transform.localPosition.x + 2.67f, 1f);
        }
    }
    public virtual void HightLightThisDoor(bool start) 
    {

        if (start)
        {
            foreach (var renderer in parameters.doorMeshRenderers)
            {
                IEnumerator cour = PingpongMaterialColorCour(renderer.material, parameters.highlightColor, "_BaseColor", parameters.highlightInterval);
                parameters.alarmCourRoutine.Add(cour);
                StartCoroutine(cour);
            }
        }
        else
        {
            foreach (var cour in parameters.alarmCourRoutine)
            {
                StopCoroutine(cour);
            }

            parameters.alarmCourRoutine.Clear();
        }
    }

    public virtual void StartAlarm(bool start)
    {
        if (start)
        {
            foreach (var renderer in parameters.alarmMeshes)
            {
                IEnumerator cour = PingpongMaterialColorCour(renderer.material, parameters.alarmColor, "_EmissionColor", parameters.alarmInterval);
                parameters.highlightCourRoutine.Add(cour);
                StartCoroutine(cour);
            }

            PlayAudio(true, parameters.alarmAudioClip);
        }
        else
        {
            foreach (var cour in parameters.highlightCourRoutine)
            {
                StopCoroutine(cour);
            }

            parameters.highlightCourRoutine.Clear();

            PlayAudio(false, parameters.alarmAudioClip);
        }
    }


    private IEnumerator PingpongMaterialColorCour(Material newMaterial, Color color, string propertyName, float interval)
    {
        Color defaultColor = newMaterial.color;
        bool changeColor = true;
        bool isPingPoing = true;

        while (isPingPoing)
        {
            newMaterial.DOColor(changeColor ? color : defaultColor, propertyName, interval);
            yield return new WaitForSeconds(interval);
            changeColor = !changeColor;
        }

        newMaterial.color = defaultColor;
    }


    private void PlayAudio(bool play, AudioClip clip)
    {
        if (clip == null) { return; }

        parameters.source.clip = clip;
        if (play)
        {
            parameters.source.Play();
        }
        else
        {
            parameters.source.Stop();
        }
    }

}


[System.Serializable]
public class DoorParameters
{
    [Header("Door Properties")]
    public string roomName = "";
    public RoomID room = RoomID.AvatarRoomMain;
    public EnvironmentPoints point = EnvironmentPoints.AvatarRoomCenter;
    public EnvironmentPoints destinationPoint = EnvironmentPoints.AvatarRoomCenter;
    public Transform leftDoor = null;
    public Transform rightDoor = null;
    public Collider doorCollider = null;

    [Header("Highligt Effect")]
    public Color highlightColor = Color.black;
    public float highlightInterval = 0.2f;
    public List<MeshRenderer> doorMeshRenderers = new List<MeshRenderer>();
    public List<IEnumerator> highlightCourRoutine = new List<IEnumerator>();

    [Header("Alarm")]
    public AudioClip alarmAudioClip = null;
    public Color alarmColor = Color.black;
    public float alarmInterval = 0.2f;
    public List<MeshRenderer> alarmMeshes = new List<MeshRenderer>();
    public List<IEnumerator> alarmCourRoutine = new List<IEnumerator>();
    public AudioSource source = null;

    public UnityEvent OnEnterDoor = new UnityEvent();
}

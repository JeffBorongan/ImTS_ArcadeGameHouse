using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTarget : MonoBehaviour
{
    #region Parameters

    [Header("Door Properties")]
    [SerializeField] private string roomName = "";
    [SerializeField] private EnvironmentPoints point = EnvironmentPoints.AvatarRoomCenter;
    [SerializeField] private EnvironmentPoints destinationPoint = EnvironmentPoints.AvatarRoomCenter;
    public Transform destination = null;
    [SerializeField] private AudioSource source = null;

    [Header("Highligt Effect")]
    [SerializeField] private Color highlightColor = Color.black;
    [SerializeField] private float highlightInterval = 0.2f;
    [SerializeField] private List<MeshRenderer> doorMeshRenderers = new List<MeshRenderer>();
    private List<IEnumerator> highlightCourRoutine = new List<IEnumerator>();

    [Header("Alarm")]
    [SerializeField] private AudioClip alarmAudioClip = null;
    [SerializeField] private Color alarmColor = Color.black;
    [SerializeField] private float alarmInterval = 0.2f;
    [SerializeField] private List<MeshRenderer> alarmMeshes = new List<MeshRenderer>();
    private List<IEnumerator> alarmCourRoutine = new List<IEnumerator>();

    [Header("Events")]
    [SerializeField] private UnityEvent OnEnterRoom = new UnityEvent();

    public string RoomName { get => roomName; }
    public EnvironmentPoints Point { get => point; }
    public EnvironmentPoints DestinationPoint { get => destinationPoint; }

    #endregion

    #region Enter Door
    public void EnterDoor(Transform player, UnityAction OnMid)
    {
        ScreenFadeManager.Instance.FadeIn(() =>
        {
            OnMid.Invoke();
            player.position = destination.position;
            ScreenFadeManager.Instance.FadeOut(() =>
            {
                if (OnEnterRoom != null)
                {
                    OnEnterRoom.Invoke();
                }
            });
        });
    }

    #endregion

    #region Highlight
    public void HightLightThisDoor(bool start)
    {
        if (start)
        {
            foreach (var renderer in doorMeshRenderers)
            {
                IEnumerator cour = PingpongMaterialColorCour(renderer.material, highlightColor, "_BaseColor", highlightInterval);
                alarmCourRoutine.Add(cour);
                StartCoroutine(cour);
            }
        }
        else
        {
            foreach (var cour in alarmCourRoutine)
            {
                StopCoroutine(cour);
            }

            alarmCourRoutine.Clear();
        }
    }

    #endregion

    #region Alarm

    public void StartAlarm(bool start)
    {
        if (start)
        {
            foreach (var renderer in alarmMeshes)
            {
                IEnumerator cour = PingpongMaterialColorCour(renderer.material, alarmColor, "_EmissionColor", alarmInterval);
                highlightCourRoutine.Add(cour);
                StartCoroutine(cour);
            }

            PlayAudio(true, alarmAudioClip);
        }
        else
        {
            foreach (var cour in highlightCourRoutine)
            {
                StopCoroutine(cour);
            }

            highlightCourRoutine.Clear();

            PlayAudio(false, alarmAudioClip);
        }
    }

    #endregion

    #region Effects
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
        if(clip == null) { return; }

        source.clip = clip;
        if (play)
        {
            source.Play();
        }
        else
        {
            source.Stop();
        }
    }

    #endregion
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTarget : MonoBehaviour
{
    [Header("Door Properties")]
    [SerializeField] private string roomName = "";
    [SerializeField] private EnvironmentPoints point = EnvironmentPoints.AvatarRoomCenter;
    [SerializeField] private EnvironmentPoints destinationPoint = EnvironmentPoints.AvatarRoomCenter;
    public Transform destination = null;

    [Header("Highligt Effect")]
    [SerializeField] private Color highlightColor = Color.black;
    [SerializeField] private float highlightInterval = 0.2f;
    [SerializeField] private List<MeshRenderer> doorMeshRenderers = new List<MeshRenderer>();
    private List<IEnumerator> highlightCourRoutine = new List<IEnumerator>();

    [Header("Alarm")]
    [SerializeField] private Color alarmColor = Color.black;
    [SerializeField] private float alarmInterval = 0.2f;
    [SerializeField] private List<MeshRenderer> alarmMeshes = new List<MeshRenderer>();
    private List<IEnumerator> alarmCourRoutine = new List<IEnumerator>();

    [Header("Events")]
    [SerializeField] private UnityEvent OnEnterRoom = new UnityEvent();

    public string RoomName { get => roomName; }
    public EnvironmentPoints Point { get => point; }
    public EnvironmentPoints DestinationPoint { get => destinationPoint; }

    public void EnterDoor(Transform player)
    {
        ScreenFadeManager.Instance.FadeIn(() =>
        {
            player.position = destination.position;
            ScreenFadeManager.Instance.FadeOut(() => 
            {
                if(OnEnterRoom != null)
                {
                    OnEnterRoom.Invoke();
                }
            });
        });
    }

    private void Start()
    {
        StartAlarm(true);
        StartCoroutine(TimeCour());
    }

    IEnumerator TimeCour()
    {
        yield return new WaitForSeconds(5f);
        StartAlarm(false);
    }

    #region Highlight
    public void HightLightThisDoor(bool start)
    {
        if (start)
        {
            foreach (var renderer in doorMeshRenderers)
            {
                IEnumerator cour = PingpongMaterialColorCour(renderer.material, highlightColor, highlightInterval);
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
                IEnumerator cour = PingpongMaterialColorCour(renderer.material, alarmColor, alarmInterval);
                highlightCourRoutine.Add(cour);
                StartCoroutine(cour);
            }
        }
        else
        {
            foreach (var cour in highlightCourRoutine)
            {
                StopCoroutine(cour);
            }

            highlightCourRoutine.Clear();
        }
    }

    #endregion

    #region Effects
    private IEnumerator PingpongMaterialColorCour(Material newMaterial, Color color, float interval)
    {
        Color defaultColor = newMaterial.color;
        bool changeColor = true;
        bool isPingPoing = true;

        while (isPingPoing)
        {
            newMaterial.DOColor(changeColor ? color : defaultColor, interval);
            yield return new WaitForSeconds(interval);
            changeColor = !changeColor;
        }

        newMaterial.color = defaultColor;
    }

    #endregion
}

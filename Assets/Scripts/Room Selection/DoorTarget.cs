using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTarget : MonoBehaviour
{
    [SerializeField] private string roomName = "";
    [SerializeField] private EnvironmentPoints point = EnvironmentPoints.AvatarRoomCenter;
    [SerializeField] private UnityEvent OnEnterRoom = new UnityEvent();
    [SerializeField] private Color highlightColor = Color.black;
    public Transform destination = null;

    public string RoomName { get => roomName; }
    public EnvironmentPoints Point { get => point; }

    [SerializeField] private List<MeshRenderer> doorMeshRenderers = new List<MeshRenderer>();

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
        HightLightThisDoor();
    }

    public void HightLightThisDoor()
    {
        foreach (var renderer in doorMeshRenderers)
        {
            HightlighDoor(renderer.material, 0.5f);
        }
    }

    private void HightlighDoor(Material newMaterial, float interval)
    {
        Color defaultColor = newMaterial.color;
        newMaterial.DOColor(highlightColor, interval).OnComplete(() =>
        {
            newMaterial.DOColor(defaultColor, interval).OnComplete(() => { HightlighDoor(newMaterial, interval); });
        });
    }
}

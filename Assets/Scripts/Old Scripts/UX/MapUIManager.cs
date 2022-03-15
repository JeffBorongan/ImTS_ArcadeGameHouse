using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUIManager : MonoBehaviour
{
    public MapUIManager Instance { private set; get; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] private List<MapElements> mapElements = new List<MapElements>();
    private Dictionary<RoomID, MapElements> mapDictionary = new Dictionary<RoomID, MapElements>();

    public Color unHighlightedMap = Color.white;
    public Color highlightedMap = Color.white;

    private void Start()
    {
        foreach (var map in mapElements)
        {
            mapDictionary.Add(map.roomID, map);
        }

        UserInteraction.Instance.OnChangeRoom.AddListener(SetCurrentRoom);
    }


    public void SetCurrentRoom(RoomID id)
    {
        foreach (var element in mapElements)
        {
            element.element.color = unHighlightedMap;
        }

        mapDictionary[id].element.color = highlightedMap;
    }

}

[System.Serializable]
public class MapElements
{
    public RoomID roomID = RoomID.AvatarRoom;
    public Image element = null;
}

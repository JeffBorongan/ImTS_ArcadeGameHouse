using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Door Guide", menuName = "Guide/Door Guide", order = 0)]

public class GoToTheDoorGuide : Guide
{
    [SerializeField] private List<EnvironmentPoints> pointsToRender = new List<EnvironmentPoints>();
    [SerializeField] private EnvironmentPoints pointToGo = EnvironmentPoints.AvatarRoomMainCenter;

    public override void ShowGuide()
    {
        List<Vector3> points = new List<Vector3>();
        foreach (var point in pointsToRender)
        {
            points.Add(Environment.Instance.environmentPoints.Where(e => e.type == point).FirstOrDefault().point.position);
        }
        EnvironmentGuideManager.Instance.RenderLine(true, points.ToArray());
    }

    public override void UnShowGuide()
    {
        EnvironmentGuideManager.Instance.RenderLine(false);
    }

    public override bool isGuideAcomplish()
    {
        return RoomInteraction.Instance.CurrentPoint == pointToGo;
    }
}

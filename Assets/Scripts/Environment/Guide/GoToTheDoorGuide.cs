using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Door Guide", menuName = "Guide/Door Guide", order = 0)]

public class GoToTheDoorGuide : Guide
{
    [SerializeField] private List<EnvironmentPoints> pointsToRender = new List<EnvironmentPoints>();
    [SerializeField] private EnvironmentPoints pointToGo = EnvironmentPoints.AvatarRoomMainCenter;

    public override void ShowGuide(UnityAction OnEndGuide)
    {
        List<Vector3> points = new List<Vector3>();
        foreach (var point in pointsToRender)
        {
            points.Add(Environment.Instance.environmentPoints.Where(e => e.type == point).FirstOrDefault().point.TransformPoint(Vector3.zero));
        }

        EnvironmentGuideManager.Instance.RenderLine(true, points.ToArray());
        EnvironmentGuideManager.Instance.StartCoroutine(GuideCour(OnEndGuide));
        Environment.Instance.DoorDictionary[pointsToRender.Last()].HightLightThisDoor(true);
        base.ShowGuide(OnEndGuide);
    }

    private IEnumerator GuideCour(UnityAction OnEnd)
    {
        while (UserInteraction.Instance.CurrentPoint != pointToGo)
        {
            yield return new WaitForEndOfFrame();
        }

        Environment.Instance.DoorDictionary[pointsToRender.Last()].HightLightThisDoor(false);
        OnEnd.Invoke();
    }

    public override void UnShowGuide()
    {
        EnvironmentGuideManager.Instance.RenderLine(false);
    }

    public override bool isGuideAcomplish()
    {
        return UserInteraction.Instance.CurrentPoint == pointToGo;
    }
}

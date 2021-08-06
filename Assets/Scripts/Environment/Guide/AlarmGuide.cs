using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Alarm Guide", menuName = "Guide/Alarm Guide", order = 2)]
public class AlarmGuide : Guide
{
    [SerializeField] private EnvironmentPoints doorToAlarm = EnvironmentPoints.AvatarRoomMainCenter;
    [SerializeField] private float alarmDuration = 2f;

    public override void ShowGuide(UnityAction OnEndGuide)
    {
        Environment.Instance.DoorDictionary[doorToAlarm].StartAlarm(true);
        EnvironmentGuideManager.Instance.StartCoroutine(TimeCour(OnEndGuide));
        base.ShowGuide(OnEndGuide);
    }

    IEnumerator TimeCour(UnityAction OnEndTime)
    {
        yield return new WaitForSeconds(alarmDuration);
        Environment.Instance.DoorDictionary[doorToAlarm].StartAlarm(false);
        OnEndTime.Invoke();
    }
}

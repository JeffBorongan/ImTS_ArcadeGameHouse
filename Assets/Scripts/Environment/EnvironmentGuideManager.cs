using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGuideManager : MonoBehaviour
{
    public static EnvironmentGuideManager Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] private TutorialActor actor = null;
    public TutorialActor Actor { get => actor; }

    public List<Guide> guides = new List<Guide>();
    private int currentGuideShown = 0;
    [SerializeField] private LineRenderer linesRenderer = null;
    [SerializeField] private GameObject lineArrowHead = null;

    public void StartGuide()
    {
        ExecuteGuide(guides[0]);
    }

    private void ExecuteGuide(Guide guide)
    {
        Debug.Log(currentGuideShown);
        guide.ShowGuide(() => 
        {
            guide.UnShowGuide();

            if (currentGuideShown + 1 < guides.Count)
            {
                currentGuideShown++;
                ExecuteGuide(guides[currentGuideShown]);
            }
            else
            {
                Debug.Log("Guide Ends");
            }
        });
    }

    public void RenderLine(bool show, Vector3[] newPoint = null)
    {
        if (newPoint == null) { return; }

        for (int i = 0; i < newPoint.Length; i++)
        {
            newPoint[i] = new Vector3(newPoint[i].x, linesRenderer.transform.position.y, newPoint[i].z);
        }

        lineArrowHead.SetActive(show);
        linesRenderer.gameObject.SetActive(show);
        linesRenderer.positionCount = newPoint.Length;
        linesRenderer.SetPositions(newPoint);

        Vector3 direction = newPoint[newPoint.Length - 1] - newPoint[newPoint.Length - 2];
        lineArrowHead.transform.rotation = Quaternion.LookRotation(direction);
        lineArrowHead.transform.position = newPoint[newPoint.Length - 1] + new Vector3(0f, linesRenderer.transform.position.y, 0f);
    }

}

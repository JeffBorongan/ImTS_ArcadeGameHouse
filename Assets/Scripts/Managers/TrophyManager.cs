using UnityEngine;

public class TrophyManager : MonoBehaviour
{
    public static TrophyManager Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private bool isGame1Accomplished = false;
    private bool isGame2Accomplished = false;
    private bool isGame3Accomplished = false;

    public bool IsGame1Accomplished { get => isGame1Accomplished; set => isGame1Accomplished = value; }
    public bool IsGame2Accomplished { get => isGame2Accomplished; set => isGame2Accomplished = value; }
    public bool IsGame3Accomplished { get => isGame3Accomplished; set => isGame3Accomplished = value; }
}
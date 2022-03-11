using UnityEngine;

public class TrophyManager : MonoBehaviour
{
    #region Singleton

    public static TrophyManager Instance { private set; get; }

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

    #endregion

    #region Parameters

    private bool isGame1Accomplished = false;
    private bool isGame2Accomplished = false;
    private bool isGame3Accomplished = false;
    private bool isGame1TrophyPresented = false;
    private bool isGame2TrophyPresented = false;
    private bool isGame3TrophyPresented = false;

    #endregion

    #region Encapsulations

    public bool IsGame1Accomplished { get => isGame1Accomplished; set => isGame1Accomplished = value; }
    public bool IsGame2Accomplished { get => isGame2Accomplished; set => isGame2Accomplished = value; }
    public bool IsGame3Accomplished { get => isGame3Accomplished; set => isGame3Accomplished = value; }
    public bool IsGame1TrophyPresented { get => isGame1TrophyPresented; set => isGame1TrophyPresented = value; }
    public bool IsGame2TrophyPresented { get => isGame2TrophyPresented; set => isGame2TrophyPresented = value; }
    public bool IsGame3TrophyPresented { get => isGame3TrophyPresented; set => isGame3TrophyPresented = value; }

    #endregion
}
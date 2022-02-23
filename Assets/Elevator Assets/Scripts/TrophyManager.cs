using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyManager : MonoBehaviour
{
    public static TrophyManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [HideInInspector] public bool isGame1Accomplished = false;
    [HideInInspector] public bool isGame2Accomplished = false;
    [HideInInspector] public bool isGame3Accomplished = false;
}
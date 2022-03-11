using DG.Tweening;
using System.Collections;
using UnityEngine;

public class AssistantBehavior : MonoBehaviour
{
    #region Singleton

    public static AssistantBehavior Instance { private set; get; }

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

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float maxUpLocation;
    [SerializeField] private float maxDownLocation;
    [SerializeField] private float originalLocation;
    private Transform player = null;
    private bool isReadyToLoop = true;

    #endregion

    #region Startup

    private void Start()
    {
        player = CharacterManager.Instance.VRCamera.transform;
    }

    private void Update()
    {
        if (isReadyToLoop)
        {
            StartCoroutine(FloatingMovement());
        }
    }

    #endregion

    #region Rodgers Actions

    private IEnumerator FloatingMovement()
    {
        isReadyToLoop = false;
        transform.DOMoveY(maxUpLocation, 3f).SetEase(Ease.Linear).OnComplete(() => transform.DOMoveY(maxDownLocation, 6f).SetEase(Ease.Linear).OnComplete(() => transform.DOMoveY(originalLocation, 3f).SetEase(Ease.Linear).OnComplete(() => isReadyToLoop = true)));
        yield return new WaitUntil(() => isReadyToLoop);
    }

    public void Speak(AudioClip clip)
    {
        transform.DOLookAt(player.position, 0.2f, AxisConstraint.Y, Vector3.up).OnComplete(() =>
        {
            audioSource.clip = clip;
            audioSource.Play();
        });
    }

    #endregion
}
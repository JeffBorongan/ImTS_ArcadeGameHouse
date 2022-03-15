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

    [SerializeField] private Animator animator = null;
    [SerializeField] private AudioSource audioSource = null;
    private Transform player = null;
    private float celebratingAnimationLength = 3f;
    private float pointingLeftAnimationLength = 3f;
    private float givingTrophyAnimationLength = 3f;
    private float pointingRightAnimationLength = 3f;
    private float greetingAnimationLength = 3f;

    #endregion

    #region Startup

    private void Start()
    {
        player = CharacterManager.Instance.VRCamera.transform;
    }

    #endregion

    #region Assistant Actions

    public void Speak(AudioClip clip)
    {
        transform.DOLookAt(player.position, 0.2f, AxisConstraint.Y, Vector3.up).OnComplete(() =>
        {
            audioSource.clip = clip;
            audioSource.Play();
        });
    }

    public void PlayCelebratingAnimation()
    {
        animator.SetBool("isCelebrating", true);
        StartCoroutine(Celebrating(celebratingAnimationLength));
    }

    public void PlayPointingLeftAnimation()
    {
        animator.SetBool("isPointingLeft", true);
        StartCoroutine(PointingLeft(pointingLeftAnimationLength));
    }

    public void PlayGivingTrophyAnimation()
    {
        animator.SetBool("isGivingTrophy", true);
        StartCoroutine(GivingTrophy(givingTrophyAnimationLength));
    }

    public void PlayPointingRightAnimation()
    {
        animator.SetBool("isPointingRight", true);
        StartCoroutine(PointingRight(pointingRightAnimationLength));
    }

    public void PlayGreetingAnimation()
    {
        animator.SetBool("isGreeting", true);
        StartCoroutine(Greeting(greetingAnimationLength));
    }

    private IEnumerator Celebrating(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isCelebrating", false);
    }

    private IEnumerator PointingLeft(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isPointingLeft", false);
    }

    private IEnumerator GivingTrophy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isGivingTrophy", false);
    }

    private IEnumerator PointingRight(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isPointingRight", false);
    }

    private IEnumerator Greeting(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isGreeting", false);
    }

    #endregion
}
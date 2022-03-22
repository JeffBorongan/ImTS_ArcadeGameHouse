using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private Transform moveAndGreet = null;
    [SerializeField] private Transform moveAndPointStart = null;
    [SerializeField] private Transform moveAndPointCustomize = null;
    [SerializeField] private Transform moveAndPointElevator = null;
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

    private IEnumerator FunctionWithDelay(float waitTime, UnityAction function)
    {
        yield return new WaitForSeconds(waitTime);
        function.Invoke();
    }

    public void Speak(AudioClip clip)
    {
        transform.DOLookAt(player.position, 0.2f, AxisConstraint.Y, Vector3.up).OnComplete(() =>
        {
            audioSource.clip = clip;
            audioSource.Play();
        });
    }

    public void MoveAndWelcomeRanger(UnityAction playWelcomeRangerClip)
    {
        transform.DOMove(moveAndGreet.position, 2f).OnComplete(() =>
        {
            PlayGreetingAnimation();
            playWelcomeRangerClip.Invoke();
            StartCoroutine(FunctionWithDelay(13.5f, () => 
            {
                transform.DOMove(moveAndPointStart.position, 2.5f);
                transform.DORotateQuaternion(moveAndPointStart.rotation, 2.5f).OnComplete(() => 
                {
                    PlayPointingLeftAnimation();
                    CharacterManager.Instance.PointersVisibility(true);
                });
            }));
        });
    }

    public void MoveAndCustomizeSuit(UnityAction playCustomizeSuitClip)
    {
        playCustomizeSuitClip.Invoke();
        transform.DOMove(moveAndPointCustomize.position, 4f);
        transform.DORotateQuaternion(moveAndPointCustomize.rotation, 4f);
        StartCoroutine(FunctionWithDelay(4f, () => 
        { 
            PlayPointingLeftAnimation();
            CharacterManager.Instance.PointersVisibility(true);
        }));
    }

    public void MoveAndPointElevator(UnityAction playGoToElevatorClip)
    {
        playGoToElevatorClip.Invoke();
        transform.DOMove(moveAndPointElevator.position, 4f);
        transform.DORotateQuaternion(moveAndPointElevator.rotation, 4f);
        StartCoroutine(FunctionWithDelay(4f, () => 
        { 
            PlayPointingRightAnimation();
            CharacterManager.Instance.PointersVisibility(true);
        }));
    }

    public void MoveAndGiveTrophy(UnityAction unityAction)
    {
        transform.DOMove(moveAndGreet.position, 2f).OnComplete(() =>
        {
            PlayCelebratingAnimation();
            StartCoroutine(FunctionWithDelay(3f, () => 
            {
                PlayGivingTrophyAnimation();
                unityAction.Invoke();
            }));
        });
    }

    #endregion

    #region Animations

    public void PlayCelebratingAnimation()
    {
        animator.SetBool("isCelebrating", true);
        StartCoroutine(Celebrating(celebratingAnimationLength));
    }

    private void PlayPointingLeftAnimation()
    {
        animator.SetBool("isPointingLeft", true);
        StartCoroutine(PointingLeft(pointingLeftAnimationLength));
    }

    private void PlayGivingTrophyAnimation()
    {
        animator.SetBool("isGivingTrophy", true);
        StartCoroutine(GivingTrophy(givingTrophyAnimationLength));
    }

    private void PlayPointingRightAnimation()
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
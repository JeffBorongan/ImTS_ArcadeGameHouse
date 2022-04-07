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
    [SerializeField] private Transform moveAndGoToCenter = null;
    [SerializeField] private Transform moveAndGoToSide = null;
    private Transform player = null;
    private float celebratingAnimationLength = 3f;
    private float pointingLeftAnimationLength = 3f;
    private float givingTrophyAnimationLength = 3f;
    private float pointingRightAnimationLength = 3f;
    private float greetingAnimationLength = 3f;

    #endregion

    #region Encapsulations

    public Animator Animator { get => animator; }

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

    public void Move(bool moveToCenter, float moveDuration, UnityAction unityAction)
    {
        Transform movePoint = null;

        if (moveToCenter)
        {
            movePoint = moveAndGoToCenter;
        }
        else
        {
            movePoint = moveAndGoToSide;
        }

        transform.DOMove(movePoint.position, moveDuration);
        transform.DORotateQuaternion(movePoint.rotation, moveDuration).OnComplete(() =>
        {
            unityAction.Invoke();
        });
    }

    public void MoveAndWelcomeRanger(UnityAction playWelcomeRangerClip)
    {
        transform.DOMove(moveAndGreet.position, 2f).OnComplete(() =>
        {
            PlayGreetingAnimation();
            playWelcomeRangerClip.Invoke();
            StartCoroutine(FunctionWithDelay(15f, () => 
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
        transform.DORotateQuaternion(moveAndPointCustomize.rotation, 4f).OnComplete(() => 
        {
            PlayPointingLeftAnimation();
            CharacterManager.Instance.PointersVisibility(true);
        });
    }

    public void MoveAndPointElevator(UnityAction playGoToElevatorClip)
    {
        playGoToElevatorClip.Invoke();
        transform.DOMove(moveAndPointElevator.position, 4f);
        transform.DORotateQuaternion(moveAndPointElevator.rotation, 4f).OnComplete(() => 
        {
            PlayPointingRightAnimation();
            CharacterManager.Instance.PointersVisibility(true);
        });
    }

    public void MoveAndGiveTrophy(GameObject vFX, UnityAction unityAction)
    {
        transform.DOMove(moveAndGreet.position, 2f).OnComplete(() =>
        {
            vFX.SetActive(true);
            PlayCelebratingAnimation();
            unityAction.Invoke();
            StartCoroutine(FunctionWithDelay(4f, () => 
            {
                vFX.SetActive(false);
                PlayGivingTrophyAnimation(); 
            }));
        });
    }

    #endregion

    #region Animations

    public void PlayCelebratingAnimation()
    {
        Animator.SetBool("isCelebrating", true);
        StartCoroutine(Celebrating(celebratingAnimationLength));
    }

    public void PlayPointingLeftAnimation()
    {
        Animator.SetBool("isPointingLeft", true);
        StartCoroutine(PointingLeft(pointingLeftAnimationLength));
    }

    private void PlayGivingTrophyAnimation()
    {
        Animator.SetBool("isGivingTrophy", true);
        StartCoroutine(GivingTrophy(givingTrophyAnimationLength));
    }

    private void PlayPointingRightAnimation()
    {
        Animator.SetBool("isPointingRight", true);
        StartCoroutine(PointingRight(pointingRightAnimationLength));
    }

    public void PlayGreetingAnimation()
    {
        Animator.SetBool("isGreeting", true);
        StartCoroutine(Greeting(greetingAnimationLength));
    }

    private IEnumerator Celebrating(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Animator.SetBool("isCelebrating", false);
    }

    private IEnumerator PointingLeft(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Animator.SetBool("isPointingLeft", false);
    }

    private IEnumerator GivingTrophy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Animator.SetBool("isGivingTrophy", false);
    }

    private IEnumerator PointingRight(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Animator.SetBool("isPointingRight", false);
    }

    private IEnumerator Greeting(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Animator.SetBool("isGreeting", false);
    }

    #endregion
}
using UnityEngine;
using UnityEngine.UI;

public class VoiceOverManager : MonoBehaviour
{
    [Header("Elevator")]
    [SerializeField] private Button btnBowlingGame = null;
    [SerializeField] private Button btnLockEmUp = null;
    [SerializeField] private Button btnWalkeyMoley = null;
    [SerializeField] private Button btnSpaceLobby = null;

    [SerializeField] private AudioClip bowlingGameClip = null;
    [SerializeField] private AudioClip lockEmUpClip = null;
    [SerializeField] private AudioClip walkeyMoleyClip = null;
    [SerializeField] private AudioClip spaceLobbyClip = null;

    [Header("Space Lobby")]
    [SerializeField] private Button btnWelcomeRanger = null;
    [SerializeField] private Button btnCustomizeSuit = null;

    [SerializeField] private AudioClip welcomeRangerClip = null;
    [SerializeField] private AudioClip customizeSuitClip = null;

    [Header("Bowling Game")]
    [SerializeField] private Button btnWelcomeGame1 = null;
    [SerializeField] private Button btnGiveInstruction = null;

    [SerializeField] private AudioClip welcomeGame1Clip = null;
    [SerializeField] private AudioClip giveInstructionClip = null;

    [Header("Lock 'Em Up")]
    [SerializeField] private Button btnPullUp = null;
    [SerializeField] private Button btnPushDown = null;
    [SerializeField] private Button btnWelcomeGame2 = null;

    [SerializeField] private AudioClip pullUpClip = null;
    [SerializeField] private AudioClip pushDownClip = null;
    [SerializeField] private AudioClip welcomeGame2Clip = null;

    [Header("Go to Elevator")]
    [SerializeField] private Button btnSpaceLobbyToElevator = null;
    [SerializeField] private Button btnBowlingGameToElevator = null;
    [SerializeField] private Button btnLockEmUpToElevator = null;

    [SerializeField] private AudioClip goToElevatorClip = null;

    private void Start()
    {
        btnBowlingGame.onClick.AddListener(() => HandleOnPlay(bowlingGameClip));
        btnLockEmUp.onClick.AddListener(() => HandleOnPlay(lockEmUpClip));
        //btnWalkeyMoley.onClick.AddListener(() => HandleOnPlay(walkeyMoleyClip));
        btnSpaceLobby.onClick.AddListener(() => HandleOnPlay(spaceLobbyClip));

        btnWelcomeRanger.onClick.AddListener(() => HandleOnPlay(welcomeRangerClip));
        btnCustomizeSuit.onClick.AddListener(() => HandleOnPlay(customizeSuitClip));

        btnWelcomeGame1.onClick.AddListener(() => HandleOnPlay(welcomeGame1Clip));
        btnGiveInstruction.onClick.AddListener(() => HandleOnPlay(giveInstructionClip));

        btnPullUp.onClick.AddListener(() => HandleOnPlay(pullUpClip));
        btnPushDown.onClick.AddListener(() => HandleOnPlay(pushDownClip));
        btnWelcomeGame2.onClick.AddListener(() => HandleOnPlay(welcomeGame2Clip));

        btnSpaceLobbyToElevator.onClick.AddListener(() => HandleOnPlay(goToElevatorClip));
        btnBowlingGameToElevator.onClick.AddListener(() => HandleOnPlay(goToElevatorClip));
        btnLockEmUpToElevator.onClick.AddListener(() => HandleOnPlay(goToElevatorClip));
    }

    private void HandleOnPlay(AudioClip clip)
    {
        AssistantBehavior.Instance.Speak(clip);
    }
}
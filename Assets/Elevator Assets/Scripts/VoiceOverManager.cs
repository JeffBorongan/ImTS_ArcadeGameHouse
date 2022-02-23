using System.Collections;
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
    //[SerializeField] private AudioClip walkeyMoleyClip = null;
    [SerializeField] private AudioClip spaceLobbyClip = null;

    [Header("Space Lobby")]
    [SerializeField] private Button btnWelcomeRanger = null;
    [SerializeField] private Button btnAreYouReady = null;
    [SerializeField] private Button btnCustomizeSuit = null;
    [SerializeField] private Button btnGoodbye = null;

    [SerializeField] private AudioClip welcomeRangerClip = null;
    [SerializeField] private AudioClip areYouReadyClip = null;
    [SerializeField] private AudioClip customizeSuitClip = null;
    [SerializeField] private AudioClip goodbyeClip = null;

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
        btnBowlingGame.onClick.AddListener(() => 
        {
            btnBowlingGame.interactable = false;
            HandleOnPlay(bowlingGameClip);
            StartCoroutine(DisableButton(bowlingGameClip.length, btnBowlingGame));
        });

        btnLockEmUp.onClick.AddListener(() => 
        {
            btnLockEmUp.interactable = false;
            HandleOnPlay(lockEmUpClip);
            StartCoroutine(DisableButton(lockEmUpClip.length, btnLockEmUp));
        });

        btnSpaceLobby.onClick.AddListener(() => 
        {
            btnSpaceLobby.interactable = false;
            HandleOnPlay(spaceLobbyClip);
            StartCoroutine(DisableButton(spaceLobbyClip.length, btnSpaceLobby));
        });





        btnWelcomeRanger.onClick.AddListener(() => 
        {
            btnWelcomeRanger.interactable = false;
            HandleOnPlay(welcomeRangerClip);
            StartCoroutine(DisableButton(welcomeRangerClip.length, btnWelcomeRanger));
        });

        btnAreYouReady.onClick.AddListener(() => 
        {
            btnAreYouReady.interactable = false;
            HandleOnPlay(areYouReadyClip);
            StartCoroutine(DisableButton(areYouReadyClip.length, btnAreYouReady));
        });

        btnCustomizeSuit.onClick.AddListener(() => 
        {
            btnCustomizeSuit.interactable = false;
            HandleOnPlay(customizeSuitClip);
            StartCoroutine(DisableButton(customizeSuitClip.length, btnCustomizeSuit));
        });

        btnGoodbye.onClick.AddListener(() => 
        {
            btnGoodbye.interactable = false;
            HandleOnPlay(goodbyeClip);
            StartCoroutine(DisableButton(goodbyeClip.length, btnGoodbye));
        });





        btnWelcomeGame1.onClick.AddListener(() => 
        {
            btnWelcomeGame1.interactable = false;
            HandleOnPlay(welcomeGame1Clip);
            StartCoroutine(DisableButton(welcomeGame1Clip.length, btnWelcomeGame1));
        });

        btnGiveInstruction.onClick.AddListener(() => 
        {
            btnGiveInstruction.interactable = false;
            HandleOnPlay(giveInstructionClip);
            StartCoroutine(DisableButton(giveInstructionClip.length, btnGiveInstruction));
        });





        btnPullUp.onClick.AddListener(() => 
        {
            btnPullUp.interactable = false;
            HandleOnPlay(pullUpClip);
            StartCoroutine(DisableButton(pullUpClip.length, btnPullUp));
        });

        btnPushDown.onClick.AddListener(() => 
        {
            btnPushDown.interactable = false;
            HandleOnPlay(pushDownClip);
            StartCoroutine(DisableButton(pushDownClip.length, btnPushDown));
        });

        btnWelcomeGame2.onClick.AddListener(() => 
        {
            btnWelcomeGame2.interactable = false;
            HandleOnPlay(welcomeGame2Clip);
            StartCoroutine(DisableButton(welcomeGame2Clip.length, btnWelcomeGame2));
        });





        btnSpaceLobbyToElevator.onClick.AddListener(() => 
        {
            btnSpaceLobbyToElevator.interactable = false;
            HandleOnPlay(goToElevatorClip);
            StartCoroutine(DisableButton(goToElevatorClip.length, btnSpaceLobbyToElevator));
        });

        btnBowlingGameToElevator.onClick.AddListener(() => 
        {
            btnBowlingGameToElevator.interactable = false;
            HandleOnPlay(goToElevatorClip);
            StartCoroutine(DisableButton(goToElevatorClip.length, btnBowlingGameToElevator));
        });

        btnLockEmUpToElevator.onClick.AddListener(() => 
        {
            btnLockEmUpToElevator.interactable = false;
            HandleOnPlay(goToElevatorClip);
            StartCoroutine(DisableButton(goToElevatorClip.length, btnLockEmUpToElevator));
        });
    }

    private void HandleOnPlay(AudioClip clip)
    {
        AssistantBehavior.Instance.Speak(clip);
    }

    IEnumerator DisableButton(float time, Button button)
    {
        yield return new WaitForSeconds(time);
        button.interactable = true;

        if (button == btnWelcomeRanger)
        {
            AvatarCustomizationManager.Instance.EnableStartButton();
            button.interactable = false;
        }

        if (button == btnGiveInstruction)
        {
            BowlingGameManagement.Instance.EnableStartButton();
            BowlingGameManagement.Instance.isBowlingGameInstructionDone = true;
        }

        if (button == btnWelcomeGame2)
        {
            SquatGameManagement.Instance.EnableStartButton();
            SquatGameManagement.Instance.isSquatGameInstructionDone = true;
        }
    }
}
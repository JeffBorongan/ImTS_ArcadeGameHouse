using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VoiceOverManager : MonoBehaviour
{
    #region Singleton

    public static VoiceOverManager Instance { private set; get; }

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

    [Header("Elevator")]
    [SerializeField] private Button btnBowlingGame = null;
    [SerializeField] private Button btnLockEmUp = null;
    [SerializeField] private Button btnSpaceLobby = null;

    [SerializeField] private AudioClip bowlingGameClip = null;
    [SerializeField] private AudioClip lockEmUpClip = null;
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
    [SerializeField] private Button btnGame1Instruction = null;

    [SerializeField] private AudioClip welcomeGame1Clip = null;
    [SerializeField] private AudioClip game1InstructionClip = null;

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

    [SerializeField] private List<Button> allButtons = new List<Button>();

    #endregion

    #region Function with Delay

    private IEnumerator FunctionWithDelay(float waitTime, UnityAction function)
    {
        yield return new WaitForSeconds(waitTime);
        function.Invoke();
    }

    #endregion

    #region Button Setup

    private void Start()
    {
        btnBowlingGame.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            HandleOnPlay(bowlingGameClip);
            StartCoroutine(FunctionWithDelay(bowlingGameClip.length, () => 
            {
                btnBowlingGame.interactable = true;
                btnSpaceLobbyToElevator.interactable = true;
                btnWelcomeGame1.interactable = true;
                btnGame1Instruction.interactable = true;
                ElevatorManager.Instance.EnableFloorButton(4, true);
            }));
        });

        btnLockEmUp.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            HandleOnPlay(lockEmUpClip);
            StartCoroutine(FunctionWithDelay(lockEmUpClip.length, () =>
            {
                btnLockEmUp.interactable = true;
                btnSpaceLobbyToElevator.interactable = true;
                btnWelcomeGame2.interactable = true;
                ElevatorManager.Instance.EnableFloorButton(2, true);
            }));
        });

        btnSpaceLobby.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            HandleOnPlay(spaceLobbyClip);
            StartCoroutine(FunctionWithDelay(spaceLobbyClip.length, () =>
            {
                btnSpaceLobby.interactable = true;
                btnBowlingGameToElevator.interactable = true;
                btnLockEmUpToElevator.interactable = true;
                ElevatorManager.Instance.EnableFloorButton(3, true);
            }));
        });





        btnWelcomeRanger.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndWelcomeRanger(() => HandleOnPlay(welcomeRangerClip));
            StartCoroutine(FunctionWithDelay(welcomeRangerClip.length, () => AnatomyCaptureManager.Instance.PnlStart.SetActive(true)));
        });

        btnAreYouReady.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            HandleOnPlay(areYouReadyClip);
            StartCoroutine(FunctionWithDelay(areYouReadyClip.length, () => ButtonsInteraction(true)));
        });

        btnCustomizeSuit.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndCustomizeSuit(() => HandleOnPlay(customizeSuitClip));
            StartCoroutine(FunctionWithDelay(customizeSuitClip.length, () => CustomizationManager.Instance.PnlCustomize.SetActive(true)));
        });

        btnGoodbye.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            HandleOnPlay(goodbyeClip);
            AssistantBehavior.Instance.PlayCelebratingAnimation();
            StartCoroutine(FunctionWithDelay(10f, () => AssistantBehavior.Instance.PlayGreetingAnimation()));
        });





        btnWelcomeGame1.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            HandleOnPlay(welcomeGame1Clip);
            StartCoroutine(FunctionWithDelay(welcomeGame1Clip.length, () => 
            {
                ButtonsInteraction(true);
                btnBowlingGameToElevator.interactable = false;
            }));
        });

        btnGame1Instruction.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            HandleOnPlay(game1InstructionClip);
            StartCoroutine(FunctionWithDelay(game1InstructionClip.length, () => BowlingGameManagement.Instance.EnableStartButton()));
        });





        btnPullUp.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            HandleOnPlay(pullUpClip);
            StartCoroutine(FunctionWithDelay(pullUpClip.length, () =>
            {
                btnPullUp.interactable = true;
                btnPushDown.interactable = true;
            }));
        });

        btnPushDown.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            HandleOnPlay(pushDownClip);
            StartCoroutine(FunctionWithDelay(pushDownClip.length, () =>
            {
                btnPullUp.interactable = true;
                btnPushDown.interactable = true;
            }));
        });

        btnWelcomeGame2.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            HandleOnPlay(welcomeGame2Clip);
            StartCoroutine(FunctionWithDelay(welcomeGame2Clip.length, () => 
            { 
                SquatGameManager.Instance.EnableStartButton();
                btnPullUp.interactable = true;
                btnPushDown.interactable = true;
            }));
        });





        btnSpaceLobbyToElevator.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndPointElevator(() => HandleOnPlay(goToElevatorClip));
            StartCoroutine(FunctionWithDelay(goToElevatorClip.length, () =>
            {
                btnSpaceLobbyToElevator.interactable = true;
                btnBowlingGame.interactable = true;
                btnLockEmUp.interactable = true;
                ElevatorManager.Instance.EnableFloorButton(1, false);
                ElevatorManager.Instance.EnableFloorButton(2, false);
                ElevatorManager.Instance.EnableFloorButton(3, false);
                ElevatorManager.Instance.EnableFloorButton(4, false);
            }));
        });

        btnBowlingGameToElevator.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndPointElevator(() => HandleOnPlay(goToElevatorClip));
            StartCoroutine(FunctionWithDelay(goToElevatorClip.length, () =>
            {
                if (TrophyManager.Instance.IsGame1Failed)
                {
                    btnAreYouReady.interactable = true;
                    btnSpaceLobbyToElevator.interactable = true;
                    btnCustomizeSuit.interactable = true;
                    btnGoodbye.interactable = true;
                }

                btnBowlingGameToElevator.interactable = true;
                btnSpaceLobby.interactable = true;
                ElevatorManager.Instance.EnableFloorButton(1, false);
                ElevatorManager.Instance.EnableFloorButton(2, false);
                ElevatorManager.Instance.EnableFloorButton(3, false);
                ElevatorManager.Instance.EnableFloorButton(4, false);
            }));
        });

        btnLockEmUpToElevator.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndPointElevator(() => HandleOnPlay(goToElevatorClip));
            StartCoroutine(FunctionWithDelay(goToElevatorClip.length, () =>
            {
                btnLockEmUpToElevator.interactable = true;
                btnSpaceLobby.interactable = true;
                ElevatorManager.Instance.EnableFloorButton(1, false);
                ElevatorManager.Instance.EnableFloorButton(2, false);
                ElevatorManager.Instance.EnableFloorButton(3, false);
                ElevatorManager.Instance.EnableFloorButton(4, false);
            }));
        });
    }

    #endregion

    #region Button Interactions

    private void HandleOnPlay(AudioClip clip)
    {
        AssistantBehavior.Instance.Speak(clip);
    }

    public void ButtonsInteraction(bool interact)
    {
        foreach (var button in allButtons)
        {
            button.interactable = interact;
        }

        btnWelcomeRanger.interactable = false;
    }

    public void ButtonsInteraction(bool interact, bool welcomeGame1, bool game1Instruction, bool welcomeGame2)
    {
        foreach (var button in allButtons)
        {
            button.interactable = interact;
        }

        btnWelcomeRanger.interactable = false;
        btnWelcomeGame1.interactable = welcomeGame1;
        btnGame1Instruction.interactable = game1Instruction;
        btnWelcomeGame2.interactable = welcomeGame2;
    }

    #endregion
}
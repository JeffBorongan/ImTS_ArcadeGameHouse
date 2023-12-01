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
    [SerializeField] private Button btnWalkeyMoley = null;
    [SerializeField] private Button btnSpaceLobby = null;

    [SerializeField] private AudioClip bowlingGameClip = null;
    [SerializeField] private AudioClip lockEmUpClip = null;
    [SerializeField] private AudioClip walkeyMoleyClip = null;
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

    [SerializeField] private AudioClip welcomeGame1LeftClip = null;
    [SerializeField] private AudioClip welcomeGame1RightClip = null;
    [SerializeField] private AudioClip welcomeGame1Clip = null;
    [SerializeField] private AudioClip game1InstructionClip = null;

    [Header("Lock 'Em Up")]
    [SerializeField] private Button btnPullUp = null;
    [SerializeField] private Button btnPushDown = null;
    [SerializeField] private Button btnWelcomeGame2 = null;

    [SerializeField] private AudioClip pullUpClip = null;
    [SerializeField] private AudioClip pushDownClip = null;
    [SerializeField] private AudioClip welcomeGame2Clip = null;

    [Header("Inventory Room")]
    [SerializeField] private Button btnGoToPlatform = null;

    [SerializeField] private AudioClip goToPlatformClip = null;

    [Header("Walkey Moley")]
    [SerializeField] private Button btnWelcomeGame3 = null;

    [SerializeField] private AudioClip welcomeGame3Clip = null;

    [Header("Go to Elevator")]
    [SerializeField] private Button btnSpaceLobbyToElevator = null;
    [SerializeField] private Button btnBowlingGameToElevator = null;
    [SerializeField] private Button btnLockEmUpToElevator = null;
    [SerializeField] private Button btnInventoryRoomToElevator = null;
    [SerializeField] private Button btnWalkeyMoleyToInventoryRoom = null;

    [SerializeField] private AudioClip goToElevatorClip = null;
    [SerializeField] private AudioClip walkeyMoleyToInventoryRoomClip = null;

    [Header("Therapist")]
    [SerializeField] private Toggle btnEncouragement = null;
    [SerializeField] private Toggle btnWhenDoingWell = null;
    [SerializeField] private Toggle btnWhenFailed = null;

    [SerializeField] private GameObject pnlEncouragement = null;
    [SerializeField] private GameObject pnlWhenDoingWell = null;
    [SerializeField] private GameObject pnlWhenFailed = null;

    [Header("Encouragement")]
    [SerializeField] private Button btnKeepGoing = null;
    [SerializeField] private Button btnThatsTheWay = null;
    [SerializeField] private Button btnJustABitMore = null;
    [SerializeField] private Button btnGoForIt = null;
    [SerializeField] private Button btnGoGoGo = null;
    [SerializeField] private Button btnYouCanWin = null;
    [SerializeField] private Button btnImCheeringForYou = null;

    [SerializeField] private AudioClip keepGoingClip = null;
    [SerializeField] private AudioClip thatsTheWayClip = null;
    [SerializeField] private AudioClip justABitMoreClip = null;
    [SerializeField] private AudioClip goForItClip = null;
    [SerializeField] private AudioClip goGoGoClip = null;
    [SerializeField] private AudioClip youCanWinClip = null;
    [SerializeField] private AudioClip imCheeringForYouClip = null;

    [Header("When Doing Well")]
    [SerializeField] private Button btnGoodJob = null;
    [SerializeField] private Button btnYoureAStar = null;
    [SerializeField] private Button btnYoureDoingWell = null;
    [SerializeField] private Button btnAwesome = null;
    [SerializeField] private Button btnCool = null;
    [SerializeField] private Button btnWellDone = null;
    [SerializeField] private Button btnYouDidIt = null;

    [SerializeField] private AudioClip goodJobClip = null;
    [SerializeField] private AudioClip youreAStarClip = null;
    [SerializeField] private AudioClip youreDoingWellClip = null;
    [SerializeField] private AudioClip awesomeClip = null;
    [SerializeField] private AudioClip coolClip = null;
    [SerializeField] private AudioClip wellDoneClip = null;
    [SerializeField] private AudioClip youDidItClip = null;

    [Header("When Failed")]
    [SerializeField] private Button btnGoodEffort = null;
    [SerializeField] private Button btnDidYouHaveFunPlayingThatGame = null;
    [SerializeField] private Button btnYouShowedFancyMovements = null;
    [SerializeField] private Button btnImProudOfYou = null;
    [SerializeField] private Button btnYoullGetThoseAliensNextTime = null;

    [SerializeField] private AudioClip goodEffortClip = null;
    [SerializeField] private AudioClip didYouHaveFunPlayingThatGameClip = null;
    [SerializeField] private AudioClip youShowedFancyMovementsClip = null;
    [SerializeField] private AudioClip imProudOfYouClip = null;
    [SerializeField] private AudioClip youllGetThoseAliensNextTimeClip = null;

    [Space][Space][Space]
    [SerializeField] private AudioSource soundSource = null;
    [SerializeField] private List<Button> allButtons = new List<Button>();
    private LastButtonSelected lastButtonSelected = LastButtonSelected.None;

    #endregion

    #region Encapsulations

    public LastButtonSelected LastButtonSelected { get => lastButtonSelected; set => lastButtonSelected = value; }

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
            LastButtonSelected = LastButtonSelected.BowlingGame;
            ButtonsInteraction(false);
            AssistantBehavior.Instance.Move(true, 4f, () => { });
            HandleOnPlay(bowlingGameClip);
            SpaceLobbyManager.Instance.OutsideSpaceLobbyOpenButton.SetActive(false);
            StartCoroutine(FunctionWithDelay(bowlingGameClip.length, () => 
            {
                InvokeLastButtonSelected();
                ElevatorManager.Instance.EnableFloorButton(4, true);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnLockEmUp.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.LockEmUp;
            ButtonsInteraction(false);
            AssistantBehavior.Instance.Move(true, 4f, () => { });
            HandleOnPlay(lockEmUpClip);
            SpaceLobbyManager.Instance.OutsideSpaceLobbyOpenButton.SetActive(false);
            StartCoroutine(FunctionWithDelay(lockEmUpClip.length, () =>
            {
                InvokeLastButtonSelected();
                ElevatorManager.Instance.EnableFloorButton(2, true);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnWalkeyMoley.onClick.AddListener(() =>
        {
            LastButtonSelected = LastButtonSelected.WalkeyMoley;
            ButtonsInteraction(false);
            AssistantBehavior.Instance.Move(true, 4f, () => { });
            HandleOnPlay(walkeyMoleyClip);
            SpaceLobbyManager.Instance.OutsideSpaceLobbyOpenButton.SetActive(false);
            StartCoroutine(FunctionWithDelay(walkeyMoleyClip.length, () =>
            {
                InvokeLastButtonSelected();
                ElevatorManager.Instance.EnableFloorButton(1, true);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnSpaceLobby.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.SpaceLobby;
            ButtonsInteraction(false);
            AssistantBehavior.Instance.Move(true, 4f, () => { });
            HandleOnPlay(spaceLobbyClip);
            StartCoroutine(FunctionWithDelay(spaceLobbyClip.length, () =>
            {
                InvokeLastButtonSelected();
                ElevatorManager.Instance.EnableFloorButton(3, true);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });





        btnWelcomeRanger.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.Any;
            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndWelcomeRanger(() => HandleOnPlay(welcomeRangerClip));
            StartCoroutine(FunctionWithDelay(welcomeRangerClip.length, () => 
            { 
                AnatomyCaptureManager.Instance.PnlStart.SetActive(true);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnAreYouReady.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.Any;
            ButtonsInteraction(false);
            HandleOnPlay(areYouReadyClip);
            StartCoroutine(FunctionWithDelay(areYouReadyClip.length, () => 
            { 
                InvokeLastButtonSelected();
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnCustomizeSuit.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.Any;
            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndCustomizeSuit(() => HandleOnPlay(customizeSuitClip));
            StartCoroutine(FunctionWithDelay(customizeSuitClip.length, () => 
            { 
                CustomizationManager.Instance.PnlCustomize.SetActive(true);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnGoodbye.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.None;
            ButtonsInteraction(false);
            HandleOnPlay(goodbyeClip);
            AssistantBehavior.Instance.PlayCelebratingAnimation();
            StartCoroutine(FunctionWithDelay(10f, () => AssistantBehavior.Instance.PlayGreetingAnimation()));
            StartCoroutine(FunctionWithDelay(goodbyeClip.length, () => 
            { 
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
                UXManager.Instance.StopSessionTimer();
            }));
        });




        

        btnGame1Instruction.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.Game1Instruction;
            ButtonsInteraction(false);
            AssistantBehavior.Instance.Move(false, 5f, () => 
            { 
                HandleOnPlay(game1InstructionClip);
                BowlingGameManagement.Instance.VideoDemo.SetActive(true);
                StartCoroutine(FunctionWithDelay(12f, () => AssistantBehavior.Instance.PlayPointingLeftAnimation()));
                StartCoroutine(FunctionWithDelay(15f, () => BowlingGameManagement.Instance.VideoDemo.SetActive(false)));
                StartCoroutine(FunctionWithDelay(game1InstructionClip.length, () =>
                {
                    InvokeLastButtonSelected();
                    BowlingGameManagement.Instance.EnableStartButton();
                    ElevatorManager.Instance.EnableFloorButton(1, false);
                    ElevatorManager.Instance.EnableFloorButton(2, false);
                    ElevatorManager.Instance.EnableFloorButton(3, false);
                    ElevatorManager.Instance.EnableFloorButton(4, false);
                    AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
                }));
            });
        });





        btnPullUp.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.Game2Controls;
            ButtonsInteraction(false);
            HandleOnPlay(pullUpClip);
            StartCoroutine(FunctionWithDelay(pullUpClip.length, () =>
            {
                InvokeLastButtonSelected();
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnPushDown.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.Game2Controls;
            ButtonsInteraction(false);
            HandleOnPlay(pushDownClip);
            StartCoroutine(FunctionWithDelay(pushDownClip.length, () =>
            {
                InvokeLastButtonSelected();
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnWelcomeGame2.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.WelcomeGame2;
            ButtonsInteraction(false);
            AssistantBehavior.Instance.Move(false, 5f, () => 
            {
                HandleOnPlay(welcomeGame2Clip);
                SquatGameManager.Instance.VideoDemo.SetActive(true);
                StartCoroutine(FunctionWithDelay(10f, () => AssistantBehavior.Instance.PlayPointingLeftAnimation()));
                StartCoroutine(FunctionWithDelay(14f, () => SquatGameManager.Instance.VideoDemo.SetActive(false)));
                StartCoroutine(FunctionWithDelay(welcomeGame2Clip.length, () =>
                {
                    InvokeLastButtonSelected();
                    SquatGameManager.Instance.EnableStartButton();
                    ElevatorManager.Instance.EnableFloorButton(1, false);
                    ElevatorManager.Instance.EnableFloorButton(2, false);
                    ElevatorManager.Instance.EnableFloorButton(3, false);
                    ElevatorManager.Instance.EnableFloorButton(4, false);
                    AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
                }));
            });
        });





        btnGoToPlatform.onClick.AddListener(() =>
        {
            LastButtonSelected = LastButtonSelected.GoToPlatform;
            ButtonsInteraction(false);
            HandleOnPlay(goToPlatformClip);
            StartCoroutine(FunctionWithDelay(goToPlatformClip.length, () =>
            {
                InvokeLastButtonSelected();
                ElevatorManager.Instance.EnableFloorButton(1, false);
                ElevatorManager.Instance.EnableFloorButton(2, false);
                ElevatorManager.Instance.EnableFloorButton(3, false);
                ElevatorManager.Instance.EnableFloorButton(4, false);
                ElevatorManager.Instance.PlayerDetection = true;
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });





        btnWelcomeGame3.onClick.AddListener(() =>
        {
            LastButtonSelected = LastButtonSelected.WelcomeGame3;
            ButtonsInteraction(false);
            PlayClip(welcomeGame3Clip);
            StartCoroutine(FunctionWithDelay(welcomeGame3Clip.length, () =>
            {
                InvokeLastButtonSelected();
                TileGameManager.Instance.EnableStartButton();
            }));
        });





        btnSpaceLobbyToElevator.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.SpaceLobbyToElevator;
            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndPointElevator(() => HandleOnPlay(goToElevatorClip));
            StartCoroutine(FunctionWithDelay(goToElevatorClip.length, () =>
            {
                InvokeLastButtonSelected();
                ElevatorManager.Instance.EnableFloorButton(1, false);
                ElevatorManager.Instance.EnableFloorButton(2, false);
                ElevatorManager.Instance.EnableFloorButton(3, false);
                ElevatorManager.Instance.EnableFloorButton(4, false);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
                SpaceLobbyManager.Instance.OutsideSpaceLobbyOpenButton.SetActive(true);
            }));
        });

        btnBowlingGameToElevator.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.GamesToElevator;

            foreach (var item in BowlingGameManagement.Instance.DisableObjects)
            {
                item.SetActive(false);
            }

            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndPointElevator(() => HandleOnPlay(goToElevatorClip));
            StartCoroutine(FunctionWithDelay(goToElevatorClip.length, () =>
            {
                InvokeLastButtonSelected();
                ElevatorManager.Instance.EnableFloorButton(1, false);
                ElevatorManager.Instance.EnableFloorButton(2, false);
                ElevatorManager.Instance.EnableFloorButton(3, false);
                ElevatorManager.Instance.EnableFloorButton(4, false);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnLockEmUpToElevator.onClick.AddListener(() => 
        {
            LastButtonSelected = LastButtonSelected.LockEmUpToElevator;

            foreach (var item in SquatGameManager.Instance.DisableObjects)
            {
                item.SetActive(false);
            }

            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndPointElevator(() => HandleOnPlay(goToElevatorClip));
            StartCoroutine(FunctionWithDelay(goToElevatorClip.length, () =>
            {
                InvokeLastButtonSelected();
                ElevatorManager.Instance.EnableFloorButton(1, false);
                ElevatorManager.Instance.EnableFloorButton(2, false);
                ElevatorManager.Instance.EnableFloorButton(3, false);
                ElevatorManager.Instance.EnableFloorButton(4, false);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnInventoryRoomToElevator.onClick.AddListener(() =>
        {
            LastButtonSelected = LastButtonSelected.GamesToElevator;
            ButtonsInteraction(false);
            AssistantBehavior.Instance.MoveAndPointElevator(() => HandleOnPlay(goToElevatorClip));
            StartCoroutine(FunctionWithDelay(goToElevatorClip.length, () =>
            {
                InvokeLastButtonSelected();
                ElevatorManager.Instance.EnableFloorButton(1, false);
                ElevatorManager.Instance.EnableFloorButton(2, false);
                ElevatorManager.Instance.EnableFloorButton(3, false);
                ElevatorManager.Instance.EnableFloorButton(4, false);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });

        btnWalkeyMoleyToInventoryRoom.onClick.AddListener(() =>
        {
            LastButtonSelected = LastButtonSelected.WalkeyMoleyToInventoryRoom;
            ButtonsInteraction(false);
            PlayClip(walkeyMoleyToInventoryRoomClip);
            StartCoroutine(FunctionWithDelay(walkeyMoleyToInventoryRoomClip.length, () =>
            {
                InvokeLastButtonSelected();
                TileGameManager.Instance.InitiateTeleport();
            }));
        });





        btnEncouragement.onValueChanged.AddListener((bool value) => 
        {
            btnWhenDoingWell.interactable = !value;
            btnWhenFailed.interactable = !value;
            pnlEncouragement.SetActive(value);
        });

        btnWhenDoingWell.onValueChanged.AddListener((bool value) =>
        {
            btnEncouragement.interactable = !value;
            btnWhenFailed.interactable = !value;
            pnlWhenDoingWell.SetActive(value);
        });

        btnWhenFailed.onValueChanged.AddListener((bool value) =>
        {
            btnEncouragement.interactable = !value;
            btnWhenDoingWell.interactable = !value;
            pnlWhenFailed.SetActive(value);
        });





        btnKeepGoing.onClick.AddListener(() => 
        {
            ButtonsInteraction(false);
            PlayClip(keepGoingClip);
            StartCoroutine(FunctionWithDelay(keepGoingClip.length, () => InvokeLastButtonSelected()));
        });

        btnThatsTheWay.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(thatsTheWayClip);
            StartCoroutine(FunctionWithDelay(thatsTheWayClip.length, () => InvokeLastButtonSelected()));
        });

        btnJustABitMore.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(justABitMoreClip);
            StartCoroutine(FunctionWithDelay(justABitMoreClip.length, () => InvokeLastButtonSelected()));
        });

        btnGoForIt.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(goForItClip);
            StartCoroutine(FunctionWithDelay(goForItClip.length, () => InvokeLastButtonSelected()));
        });

        btnGoGoGo.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(goGoGoClip);
            StartCoroutine(FunctionWithDelay(goGoGoClip.length, () => InvokeLastButtonSelected()));
        });

        btnYouCanWin.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(youCanWinClip);
            StartCoroutine(FunctionWithDelay(youCanWinClip.length, () => InvokeLastButtonSelected()));
        });

        btnImCheeringForYou.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(imCheeringForYouClip);
            StartCoroutine(FunctionWithDelay(imCheeringForYouClip.length, () => InvokeLastButtonSelected()));
        });





        btnGoodJob.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(goodJobClip);
            StartCoroutine(FunctionWithDelay(goodJobClip.length, () => InvokeLastButtonSelected()));
        });

        btnYoureAStar.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(youreAStarClip);
            StartCoroutine(FunctionWithDelay(youreAStarClip.length, () => InvokeLastButtonSelected()));
        });

        btnYoureDoingWell.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(youreDoingWellClip);
            StartCoroutine(FunctionWithDelay(youreDoingWellClip.length, () => InvokeLastButtonSelected()));
        });

        btnAwesome.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(awesomeClip);
            StartCoroutine(FunctionWithDelay(awesomeClip.length, () => InvokeLastButtonSelected()));
        });

        btnCool.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(coolClip);
            StartCoroutine(FunctionWithDelay(coolClip.length, () => InvokeLastButtonSelected()));
        });

        btnWellDone.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(wellDoneClip);
            StartCoroutine(FunctionWithDelay(wellDoneClip.length, () => InvokeLastButtonSelected()));
        });

        btnYouDidIt.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(youDidItClip);
            StartCoroutine(FunctionWithDelay(youDidItClip.length, () => InvokeLastButtonSelected()));
        });





        btnGoodEffort.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(goodEffortClip);
            StartCoroutine(FunctionWithDelay(goodEffortClip.length, () => InvokeLastButtonSelected()));
        });

        btnDidYouHaveFunPlayingThatGame.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(didYouHaveFunPlayingThatGameClip);
            StartCoroutine(FunctionWithDelay(didYouHaveFunPlayingThatGameClip.length, () => InvokeLastButtonSelected()));
        });

        btnYouShowedFancyMovements.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(youShowedFancyMovementsClip);
            StartCoroutine(FunctionWithDelay(youShowedFancyMovementsClip.length, () => InvokeLastButtonSelected()));
        });

        btnImProudOfYou.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(imProudOfYouClip);
            StartCoroutine(FunctionWithDelay(imProudOfYouClip.length, () => InvokeLastButtonSelected()));
        });

        btnYoullGetThoseAliensNextTime.onClick.AddListener(() =>
        {
            ButtonsInteraction(false);
            PlayClip(youllGetThoseAliensNextTimeClip);
            StartCoroutine(FunctionWithDelay(youllGetThoseAliensNextTimeClip.length, () => InvokeLastButtonSelected()));
        });
    }

    #endregion

    #region Button Interactions

    public void PlayClip(AudioClip clip)
    {
        soundSource.clip = clip;
        soundSource.Play();
    }

    private void HandleOnPlay(AudioClip clip)
    {
        AssistantBehavior.Instance.Speak(clip);
        AssistantBehavior.Instance.Animator.SetBool("isBlinking", true);
    }

    public void ButtonsInteraction(bool interact)
    {
        foreach (var button in allButtons)
        {
            button.interactable = interact;
        }

        btnWelcomeRanger.interactable = false;
    }

    public void ButtonsInteraction(bool interact, bool welcomeGame1, bool game1Instruction, bool welcomeGame2, bool goToPlatform, bool welcomeGame3)
    {
        foreach (var button in allButtons)
        {
            button.interactable = interact;
        }

        btnWelcomeRanger.interactable = false;
        btnWelcomeGame1.interactable = welcomeGame1;
        btnGame1Instruction.interactable = game1Instruction;
        btnWelcomeGame2.interactable = welcomeGame2;
        btnGoToPlatform.interactable = goToPlatform;
        btnWelcomeGame3.interactable = welcomeGame3;
    }

    public void InvokeLastButtonSelected()
    {
        if (LastButtonSelected == LastButtonSelected.Any)
        {
            ButtonsInteraction(true);
        }

        if (LastButtonSelected == LastButtonSelected.SpaceLobbyToElevator)
        {
            ButtonsInteraction(true);
            btnAreYouReady.interactable = false;
            btnCustomizeSuit.interactable = false;
            btnGoodbye.interactable = false;
            btnSpaceLobby.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.BowlingGame)
        {
            ButtonsInteraction(true);
            btnAreYouReady.interactable = false;
            btnCustomizeSuit.interactable = false;
            btnGoodbye.interactable = false;
            btnLockEmUp.interactable = false;
            btnWalkeyMoley.interactable = false;
            btnSpaceLobby.interactable = false;
            btnBowlingGameToElevator.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.WelcomeGame1)
        {
            ButtonsInteraction(true);
            btnLockEmUp.interactable = false;
            btnWalkeyMoley.interactable = false;
            btnSpaceLobby.interactable = false;
            btnBowlingGameToElevator.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.Game1Instruction)
        {
            ButtonsInteraction(true, false, false, false, false, false);
            btnBowlingGame.interactable = false;
            btnLockEmUp.interactable = false;
            btnWalkeyMoley.interactable = false;
            btnSpaceLobby.interactable = false;
            btnBowlingGameToElevator.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.GamesToElevator)
        {
            ButtonsInteraction(true, false, false, false, false, false);
            btnBowlingGame.interactable = false;
            btnLockEmUp.interactable = false;
            btnWalkeyMoley.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.SpaceLobby)
        {
            ButtonsInteraction(true, false, false, false, false, false);

            if (TrophyManager.Instance.IsGame1Failed)
            {
                btnBowlingGame.interactable = false;
                btnLockEmUp.interactable = false;
                btnWalkeyMoley.interactable = false;
            }

            else if (TrophyManager.Instance.IsGame3Failed)
            {
                btnBowlingGame.interactable = false;
                btnLockEmUp.interactable = false;
                btnWalkeyMoley.interactable = false;
            }

            else
            {
                btnAreYouReady.interactable = false;
                btnCustomizeSuit.interactable = false;
                btnGoodbye.interactable = false;
                btnSpaceLobbyToElevator.interactable = false;
                btnBowlingGame.interactable = false;
                btnLockEmUp.interactable = false;
                btnWalkeyMoley.interactable = false;
            }
        }

        if (LastButtonSelected == LastButtonSelected.LockEmUp)
        {
            ButtonsInteraction(true);
            btnAreYouReady.interactable = false;
            btnCustomizeSuit.interactable = false;
            btnGoodbye.interactable = false;
            btnBowlingGame.interactable = false;
            btnWalkeyMoley.interactable = false;
            btnSpaceLobby.interactable = false;
            btnPullUp.interactable = false;
            btnPushDown.interactable = false;
            btnLockEmUpToElevator.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.WelcomeGame2)
        {
            ButtonsInteraction(true);
            btnBowlingGame.interactable = false;
            btnLockEmUp.interactable = false;
            btnWalkeyMoley.interactable = false;
            btnSpaceLobby.interactable = false;
            btnPullUp.interactable = false;
            btnPushDown.interactable = false;
            btnWelcomeGame2.interactable = false;
            btnLockEmUpToElevator.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.Game2Controls)
        {
            ButtonsInteraction(true);
            btnWelcomeGame2.interactable = false;
            btnLockEmUpToElevator.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.LockEmUpToElevator)
        {
            ButtonsInteraction(true, false, false, false, false, false);
            btnPullUp.interactable = false;
            btnPushDown.interactable = false;
            btnBowlingGame.interactable = false;
            btnLockEmUp.interactable = false;
            btnWalkeyMoley.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.WalkeyMoley)
        {
            ButtonsInteraction(true);
            btnAreYouReady.interactable = false;
            btnCustomizeSuit.interactable = false;
            btnGoodbye.interactable = false;
            btnBowlingGame.interactable = false;
            btnLockEmUp.interactable = false;
            btnSpaceLobby.interactable = false;
            btnInventoryRoomToElevator.interactable = false;
            btnWalkeyMoleyToInventoryRoom.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.GoToPlatform)
        {
            ButtonsInteraction(true);
            btnBowlingGame.interactable = false;
            btnLockEmUp.interactable = false;
            btnWalkeyMoley.interactable = false;
            btnSpaceLobby.interactable = false;
            btnInventoryRoomToElevator.interactable = false;
            btnWalkeyMoleyToInventoryRoom.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.WelcomeGame3)
        {
            ButtonsInteraction(true);
            btnWelcomeGame3.interactable = false;
            btnWalkeyMoleyToInventoryRoom.interactable = false;
        }

        if (LastButtonSelected == LastButtonSelected.WalkeyMoleyToInventoryRoom)
        {
            ButtonsInteraction(true, false, false, false, false, false);
            btnWalkeyMoleyToInventoryRoom.interactable = true;
        }
    }

    #endregion

    #region Helper Functions
    public void HandleGame1AudioClip(int legSelected)
    {
        welcomeGame1Clip = legSelected == 0 ? welcomeGame1LeftClip : welcomeGame1RightClip;

        btnWelcomeGame1.onClick.AddListener(() =>
        {
            LastButtonSelected = LastButtonSelected.WelcomeGame1;
            ButtonsInteraction(false);
            HandleOnPlay(welcomeGame1Clip);
            StartCoroutine(FunctionWithDelay(welcomeGame1Clip.length, () =>
            {
                InvokeLastButtonSelected();
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
            }));
        });
    }
    #endregion
}
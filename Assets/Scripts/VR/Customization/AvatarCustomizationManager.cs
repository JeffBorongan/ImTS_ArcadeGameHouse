using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AvatarCustomizationManager : MonoBehaviour
{
    #region Singleton
    public static AvatarCustomizationManager Instance { private set; get; }

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

    #region Paramaters

    [Header("Start Screen")]
    [SerializeField] private GameObject pnlStartScreen = null;
    [SerializeField] private Button btnStart = null;

    [Header("Anatomy Capture")]
    [SerializeField] private GameObject pnlAnatomyCapture = null;
    [SerializeField] private Transform vrCameraPoint = null;
    [SerializeField] private Transform vrLeftHandPoint = null;
    [SerializeField] private Transform vrRightHandPoint = null;
    [SerializeField] private CharacterCustomization character = null;
    [SerializeField] private CharacterCustomization characterMimic = null;

    [Space]
    [SerializeField] private string captureHeightMessage = "";
    [SerializeField] private Sprite imgCaptureHeight = null;
    [SerializeField] private float captureSensitivity = 0.01f;
    [SerializeField] private float captureProgressSentivity = 10f;
    [SerializeField] private Slider progressBar = null;

    public DictionaryEvent OnUpdateAnatomy = new DictionaryEvent();

    [Header("Leg Selection")]
    [SerializeField] private GameObject pnlLegSelection = null;
    [SerializeField] private Button btnLeftLegSelect = null;
    [SerializeField] private Button btnRightLegSelect = null;

    public IntEvent OnLegSelect = new IntEvent();

    [Header("Customization")]
    [SerializeField] private GameObject pnlCustomization = null;

    [Space]
    [SerializeField] private UIBodyPart bodyPartPrefab = null;
    [SerializeField] private Transform bodyPartVertical1 = null;
    [SerializeField] private Transform bodyPartVertical2 = null;

    [Space] 
    [SerializeField] private TextMeshProUGUI txtLabel = null;
    [SerializeField] private List<UIBodyPartCustom> bodyPartsCustom = new List<UIBodyPartCustom>();

    private BodyPartCustomization currentSelectedBodyPart = null;

    [Space]
    [SerializeField] private GameObject pnlConfirmPurchase = null;
    [SerializeField] private Button btnConfirmPurchase = null;


    [Header("Overlay")]
    [SerializeField] private GameObject pnlOverlay = null;
    [SerializeField] private TextMeshProUGUI txtMessage = null;
    [SerializeField] private Image imgMessageImage = null;

    [Header("Transistion")]
    [SerializeField] private float transistionDuration = 0f;
    private PanelCustomization currentPanel = PanelCustomization.StartScreen;
    Dictionary<PanelCustomization, GameObject> panels = new Dictionary<PanelCustomization, GameObject>();

    #endregion

    #region Start

    private void Start()
    {
        GameExecution.Instance.OnInitialize.AddListener(() =>
        {
            panels.Add(PanelCustomization.StartScreen, pnlStartScreen);
            panels.Add(PanelCustomization.AnatomyCapture, pnlAnatomyCapture);
            panels.Add(PanelCustomization.Overlay, pnlOverlay);
            panels.Add(PanelCustomization.LegSelection, pnlLegSelection);
            panels.Add(PanelCustomization.Customization, pnlCustomization);

            List<BodyPartCustomization> vertical1 = new List<BodyPartCustomization>();
            List<BodyPartCustomization> vertical2 = new List<BodyPartCustomization>();

            for (int i = 0; i < CustomizationShopManager.Instance.BodyPartCustomization.Count; i++)
            {
                if (i < 4)
                {
                    vertical1.Add(CustomizationShopManager.Instance.BodyPartCustomization[i]);
                }
                else
                {
                    vertical2.Add(CustomizationShopManager.Instance.BodyPartCustomization[i]);
                }
            }

            InitializeCustomization(vertical1, true);
            InitializeCustomization(vertical2, false);
        });

        GameExecution.Instance.OnEventRegistration.AddListener(() =>
        {
            btnStart.onClick.AddListener(HandleOnPressStart);
            btnLeftLegSelect.onClick.AddListener(() => HandleOnLegSelect(0));
            btnRightLegSelect.onClick.AddListener(() => HandleOnLegSelect(1));

            UserDataManager.Instance.OnUserDataUpdate.AddListener(HandleOnUserDataUpdate);
        });
    }


    #endregion

    #region Start Screen

    private void HandleOnPressStart()
    {
        Transistion(PanelCustomization.LegSelection);
    }

    #endregion

    #region Anatomy Capture

    IEnumerator BodyAnatomyCapture(UnityAction OnEndAction)
    {
        yield return new WaitForSeconds(1f);

        Vector3 headPos = vrCameraPoint.position;
        Vector3 leftHandPos = vrLeftHandPoint.position;
        Vector3 rightHandPos = vrRightHandPoint.position;
        float progress = 0f;

        while (progress < 100f)
        {
            Vector3 headMagnitude = vrCameraPoint.position - headPos;
            Vector3 leftHandMagnitude = vrLeftHandPoint.position - leftHandPos;
            Vector3 rightHandMagnitude = vrRightHandPoint.position - rightHandPos;

            headPos = vrCameraPoint.position;
            leftHandPos = vrLeftHandPoint.position;
            rightHandPos = vrRightHandPoint.position;

            if (headMagnitude.magnitude < captureSensitivity && leftHandMagnitude.magnitude < captureSensitivity && rightHandMagnitude.magnitude < captureSensitivity)
            {
                progress += Time.deltaTime * captureProgressSentivity;
            }
            else
            {
                progress = 0f;
            }

            progressBar.value = progress / 100;
            yield return new WaitForEndOfFrame();
        }
        OnEndAction.Invoke();
    }

    private void AdjustHeight()
    {
        character.gameObject.SetActive(true);
        character.SetHeight();

        if (!characterMimic.gameObject.activeSelf)
        {
            characterMimic.transform.localScale = character.transform.localScale;
            characterMimic.gameObject.SetActive(true);
        }
    }

    #endregion

    #region Leg Selection

    private void HandleOnLegSelect(int leg)
    {
        OnLegSelect.Invoke(leg);

        Transistion(PanelCustomization.Overlay);

        ShowMessage(captureHeightMessage, imgCaptureHeight, 5f, () =>
        {
            Transistion(PanelCustomization.AnatomyCapture);

            StartCoroutine(BodyAnatomyCapture(() =>
            {
                pnlCustomization.transform.position = new Vector3(pnlCustomization.transform.position.x, vrCameraPoint.position.y, pnlCustomization.transform.position.z);

                Dictionary<string, Vector3> currentAnatomy = new Dictionary<string, Vector3>();
                currentAnatomy.Add(AnatomyPart.Head.ToString(), vrCameraPoint.position);
                currentAnatomy.Add(AnatomyPart.LeftHand.ToString(), vrLeftHandPoint.localPosition);
                currentAnatomy.Add(AnatomyPart.RightHand.ToString(), vrRightHandPoint.localPosition);

                AdjustHeight();

                OnUpdateAnatomy.Invoke(currentAnatomy);
                Environment.Instance.CurrentAnatomy = currentAnatomy;

                Transistion(PanelCustomization.Customization);
            }));
        });
    }

    #endregion

    #region Customization

    public void InitializeCustomization(List<BodyPartCustomization> bodyParts, bool vertical)
    {
        foreach (var bodyPart in bodyParts)
        {
            GameObject clone = Instantiate(bodyPartPrefab.gameObject, vertical ? bodyPartVertical1 : bodyPartVertical2);
            UIBodyPart uIBodyPart = clone.GetComponent<UIBodyPart>();
            uIBodyPart.txtLabel.text = bodyPart.bodyPartID.ToString();

            uIBodyPart.btnBodyPart.onClick.AddListener(() => {
                List<Material> materials = new List<Material>();
                List<Sprite> sprites = new List<Sprite>();

                foreach (var profile in bodyPart.bodyPartProfile)
                {
                    materials.Add(profile.bodyPartMaterial);
                }

                BodyPartID id = bodyPart.bodyPartID;

                HandleOnSelectBodyPart(id);
            });

            clone.name = bodyPart.bodyPartID.ToString();
            clone.SetActive(true);
        }
    }

    private void HandleOnSelectBodyPart(BodyPartID id)
    {
        OpenBodyPartSelections(character, characterMimic, id);
    }

    private void HandleOnUserDataUpdate(UserData data)
    {
        if (currentSelectedBodyPart == null) { return; }

        for (int i = 0; i < currentSelectedBodyPart.bodyPartProfile.Count; i++)
        {
            bodyPartsCustom[i].imgBodyPartPreview.sprite = currentSelectedBodyPart.bodyPartProfile[i].bodyPartSpriteUI;
            bodyPartsCustom[i].txtStarCost.text = currentSelectedBodyPart.bodyPartProfile[i].starCost.ToString();

            if (CustomizationShopManager.Instance.IsPurchased(currentSelectedBodyPart.bodyPartProfile[i]))
            {
                bodyPartsCustom[i].imgLocked.gameObject.SetActive(false);
                bodyPartsCustom[i].currency.SetActive(false);
            }
            else
            {
                bodyPartsCustom[i].imgLocked.gameObject.SetActive(true);
                bodyPartsCustom[i].currency.SetActive(true);
            }
        }
    }

    public void OpenBodyPartSelections(CharacterCustomization character, CharacterCustomization characterMimic, BodyPartID id)
    {
        txtLabel.text = id.ToString();

        BodyPart bodyPart = character.bodyParts.Where(b => b.id == id).FirstOrDefault();
        BodyPart bodyPartMimic = characterMimic.bodyParts.Where(b => b.id == id).FirstOrDefault();

        foreach (var ui in bodyPartsCustom)
        {
            ui.gameObject.SetActive(false);
        }

        currentSelectedBodyPart = CustomizationShopManager.Instance.BodyPartCustomization.Where(b => b.bodyPartID == id).FirstOrDefault();

        for (int i = 0; i < currentSelectedBodyPart.bodyPartProfile.Count; i++)
        {
            bodyPartsCustom[i].imgBodyPartPreview.sprite = currentSelectedBodyPart.bodyPartProfile[i].bodyPartSpriteUI;
            bodyPartsCustom[i].txtStarCost.text = currentSelectedBodyPart.bodyPartProfile[i].starCost.ToString();

            if (CustomizationShopManager.Instance.IsPurchased(currentSelectedBodyPart.bodyPartProfile[i]))
            {
                bodyPartsCustom[i].imgLocked.gameObject.SetActive(false);
                bodyPartsCustom[i].currency.SetActive(false);
            }
            else
            {
                bodyPartsCustom[i].imgLocked.gameObject.SetActive(true);
                bodyPartsCustom[i].currency.SetActive(true);
            }

            bodyPartsCustom[i].btnBodyPartCustom.onClick.RemoveAllListeners();
            BodyPartCustomizationProfile profile = currentSelectedBodyPart.bodyPartProfile[i];
            bodyPartsCustom[i].btnBodyPartCustom.onClick.AddListener(() =>
            {
                Material newMaterial = profile.bodyPartMaterial;

                if (CustomizationShopManager.Instance.IsPurchased(profile))
                {
                    bodyPart.ChangeMaterial(newMaterial);
                    bodyPartMimic.ChangeMaterial(newMaterial);
                }
                else if (CustomizationShopManager.Instance.CanBePurchased(profile))
                {
                    pnlConfirmPurchase.SetActive(true);

                    btnConfirmPurchase.onClick.RemoveAllListeners();
                    btnConfirmPurchase.onClick.AddListener(() =>
                    {
                        CustomizationShopManager.Instance.Buy(profile);
                        bodyPartsCustom[i].currency.SetActive(false);
                        pnlConfirmPurchase.SetActive(false);
                    });
                }
            });

            bodyPartsCustom[i].gameObject.SetActive(true);
        }
    }    

    #endregion

    #region Overlay

    public void ShowMessage(string message, Sprite image, float duration, UnityAction OnComplete)
    {
        if(image != null)
        {
            imgMessageImage.gameObject.SetActive(true);
            imgMessageImage.sprite = image;
            imgMessageImage.DOFade(1f, 0.5f);
            imgMessageImage.DOFade(0f, 0.5f).SetDelay(duration + 0.5f);
        }

        txtMessage.gameObject.SetActive(true);
        txtMessage.text = message;
        txtMessage.DOFade(1f, 0.5f).OnComplete(() =>
        {
            txtMessage.DOFade(0f, 0.5f).SetDelay(duration).OnComplete(() =>
            { 
                imgMessageImage.gameObject.SetActive(false);
                txtMessage.gameObject.SetActive(false);
                OnComplete.Invoke();
            });
        });
    }

    #endregion

    #region Transistion

    public void Transistion(PanelCustomization to)
    {
        panels[currentPanel].SetActive(false);
        panels[to].SetActive(true);
        currentPanel = to;
    }

    #endregion

    #region Teleport

    public void TeleportToNextRoom()
    {
        transform.position = Environment.Instance.PointsDictionary[EnvironmentPoints.AvatarRoomCenter].point.position;
        transform.rotation = Environment.Instance.PointsDictionary[EnvironmentPoints.AvatarRoomCenter].point.rotation;
    }

    #endregion
}

public enum PanelCustomization
{
    StartScreen,
    AnatomyCapture,
    Overlay,
    LegSelection,
    Customization
}
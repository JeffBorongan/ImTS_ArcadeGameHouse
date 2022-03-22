using UnityEngine;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour
{
    #region Singleton

    public static CustomizationManager Instance { private set; get; }

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

    [SerializeField] private GameObject pnlCustomize = null;
    [SerializeField] private GameObject pnlCategorySelection = null;
    [SerializeField] private GameObject pnlHelmetCustomization = null;
    [SerializeField] private GameObject pnlTorsoCustomization = null;
    [SerializeField] private GameObject pnlArmsCustomization = null;
    [SerializeField] private GameObject pnlBootsCustomization = null;
    [SerializeField] private GameObject pnlConfirmCustomization = null;
    [SerializeField] private Button btnCustomize = null;
    [SerializeField] private Button btnHelmetCategory = null;
    [SerializeField] private Button btnTorsoCategory = null;
    [SerializeField] private Button btnArmsCategory = null;
    [SerializeField] private Button btnBootsCategory = null;
    [SerializeField] private Button btnConfirm = null;
    private int enableHelmet = 6;
    private int enableTorso = 9;
    private int enableArms = 0;
    private int enableBoots = 3;

    #endregion

    #region Encapsulations

    public GameObject PnlCustomize { get => pnlCustomize; }

    #endregion

    #region Button Setup

    private void Start()
    {
        btnCustomize.onClick.AddListener(() =>
        {
            pnlCategorySelection.SetActive(true);
            pnlConfirmCustomization.SetActive(true);
            PnlCustomize.SetActive(false);
        });

        btnHelmetCategory.onClick.AddListener(() => 
        {
            pnlHelmetCustomization.SetActive(true);
            pnlTorsoCustomization.SetActive(false);
            pnlArmsCustomization.SetActive(false);
            pnlBootsCustomization.SetActive(false);
        });

        btnTorsoCategory.onClick.AddListener(() =>
        {
            pnlTorsoCustomization.SetActive(true);
            pnlHelmetCustomization.SetActive(false);
            pnlArmsCustomization.SetActive(false);
            pnlBootsCustomization.SetActive(false);
        });

        btnArmsCategory.onClick.AddListener(() =>
        {
            pnlArmsCustomization.SetActive(true);
            pnlTorsoCustomization.SetActive(false);
            pnlHelmetCustomization.SetActive(false);
            pnlBootsCustomization.SetActive(false);
        });

        btnBootsCategory.onClick.AddListener(() =>
        {
            pnlBootsCustomization.SetActive(true);
            pnlArmsCustomization.SetActive(false);
            pnlTorsoCustomization.SetActive(false);
            pnlHelmetCustomization.SetActive(false);
        });

        btnConfirm.onClick.AddListener(() =>
        {
            for (int index = 0; index < CharacterManager.Instance.SuitCustomization.SuitParts.Count; index++)
            {
                CharacterManager.Instance.SuitCustomization.SuitParts[index].SetActive(false);
            }

            CharacterManager.Instance.SuitCustomization.SuitParts[enableHelmet].SetActive(true);
            CharacterManager.Instance.SuitCustomization.SuitParts[enableTorso].SetActive(true);
            CharacterManager.Instance.SuitCustomization.SuitParts[enableArms].SetActive(true);
            CharacterManager.Instance.SuitCustomization.SuitParts[enableBoots].SetActive(true);
            CharacterManager.Instance.PointersVisibility(false);
            VoiceOverManager.Instance.ButtonsInteraction(true);

            PnlCustomize.SetActive(false);
            pnlCategorySelection.SetActive(false);
            pnlHelmetCustomization.SetActive(false);
            pnlTorsoCustomization.SetActive(false);
            pnlArmsCustomization.SetActive(false);
            pnlBootsCustomization.SetActive(false);
            pnlConfirmCustomization.SetActive(false);
        });
    }

    #endregion

    #region Choose Suit Part

    public void ChooseHelmetPart(int enable)
    {
        enableHelmet = enable;
    }

    public void ChooseTorsoPart(int enable)
    {
        enableTorso = enable;
    }

    public void ChooseArmsPart(int enable)
    {
        enableArms = enable;
    }

    public void ChooseBootsPart(int enable)
    {
        enableBoots = enable;
    }

    #endregion
}
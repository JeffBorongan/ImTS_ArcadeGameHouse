using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBodyPartSelection : MonoBehaviour
{
    public static UIBodyPartSelection Instance { private set; get; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] private CharacterCustomization character = null;
    [SerializeField] private CharacterCustomization characterMimic = null;
    [SerializeField] private UIBodyPart bodyPartPrefab = null;
    [SerializeField] private Transform bodyPartVertical1 = null;
    [SerializeField] private Transform bodyPartVertical2 = null;

    private void Start()
    {
        UIBodyPartCustomization.Instance.OpenBodyPartSelections(character, characterMimic, BodyPartID.HELMET);
    }

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
        UIBodyPartCustomization.Instance.OpenBodyPartSelections(character, characterMimic, id);
    }
}



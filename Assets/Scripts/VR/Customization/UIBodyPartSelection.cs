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
    [SerializeField] private UIBodyPart bodyPartPrefab = null;
    [SerializeField] private Transform bodyPartParent = null;
    [SerializeField] private Button btnAdjustHeight = null;

    private void Start()
    {
        foreach (var bodyPart in character.bodyParts)
        {
            GameObject clone = Instantiate(bodyPartPrefab.gameObject, bodyPartParent);
            UIBodyPart uIBodyPart = clone.GetComponent<UIBodyPart>();
            uIBodyPart.txtLabel.text = bodyPart.id.ToString();
            uIBodyPart.btnBodyPart.onClick.AddListener(() => HandleOnSelectBodyPart(bodyPart.id));

            clone.name = bodyPart.id.ToString();
            clone.SetActive(true);
        }

        btnAdjustHeight.onClick.AddListener(HandleOnAdjustHeight);

        UIBodyPartCustomization.Instance.OpenBodyPartSelections(character, 0);
    }

    private void HandleOnSelectBodyPart(BodyPartID id)
    {
        UIBodyPartCustomization.Instance.OpenBodyPartSelections(character, id);
    }

    private void HandleOnAdjustHeight()
    {
        character.SetHeight();
    }



}



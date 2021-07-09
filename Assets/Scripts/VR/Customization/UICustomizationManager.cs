using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomizationManager : MonoBehaviour
{
    public static UICustomizationManager Instance { private set; get; }

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

            uIBodyPart.btnNext.onClick.AddListener(() => ChangeBodyPart(true, bodyPart.id, uIBodyPart));
            uIBodyPart.btnPrevious.onClick.AddListener(() => ChangeBodyPart(false, bodyPart.id, uIBodyPart));

            clone.name = bodyPart.id.ToString();
            clone.SetActive(true);
        }

        btnAdjustHeight.onClick.AddListener(HandleOnAdjustHeight);
    }

    private void HandleOnAdjustHeight()
    {
        character.SetHeight();
    }

    private void ChangeBodyPart(bool next, BodyPartID id, UIBodyPart ui)
    {
        character.ChangeBodyPart(next, id, ui);
    }

}



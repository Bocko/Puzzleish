using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Item Pick")]
    public Image pointer;
    public TextMeshProUGUI itemName;
    public Color highlightedPointerColor;

    [Header("Stance Indicator")]
    public Image stanceHolder;
    public Sprite standing;
    public Sprite crouching;
    bool crouched;

    [Header("Jetpack")]
    public RectTransform jetpackFuelBarRect;
    public Image jetpackFuelBarImage;
    public GameObject jetpackUIHolder;
    public Gradient fuelGradient;
    bool isOn;

    PlayerItemPickUper playerItemPickUper;
    PlayerMovement playerMovement;
    PlayerJetpack playerJetpack;

    Color defaultPointerColor;

    void Start()
    {
        playerItemPickUper = FindObjectOfType<PlayerItemPickUper>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerJetpack = FindObjectOfType<PlayerJetpack>();

        itemName.text = "";
        defaultPointerColor = pointer.color;

        crouched = playerMovement.isCrouched;

        isOn = playerJetpack.isOn;
        jetpackUIHolder.SetActive(isOn);
    }

    void Update()
    {
        UpdatePointer();
        UpdateStanceUI();
        UpdateJetpackUI();
    }

    void UpdatePointer()
    {
        itemName.text = playerItemPickUper.currentItemName;
        if (playerItemPickUper.moveable)
        {
            pointer.color = highlightedPointerColor;
        }
        else
        {
            pointer.color = defaultPointerColor;
        }
    }

    void UpdateStanceUI()
    {
        if (crouched != playerMovement.isCrouched)
        {
            crouched = playerMovement.isCrouched;
            if (crouched)
            {
                stanceHolder.sprite = crouching;
            }
            else
            {
                stanceHolder.sprite = standing;
            }
        }
    }

    void UpdateJetpackUI()
    {
        if(isOn != playerJetpack.isOn)
        {
            isOn = playerJetpack.isOn;
            jetpackUIHolder.SetActive(isOn);
        }
        float fuelPercent = playerJetpack.fuel / playerJetpack.maxFuel;
        jetpackFuelBarRect.localScale = new Vector3(fuelPercent, 1, 1);
        jetpackFuelBarImage.color = GetColorFromGradient(fuelPercent);
    }

    Color GetColorFromGradient(float value)
    {
        return fuelGradient.Evaluate(value);
    }
}

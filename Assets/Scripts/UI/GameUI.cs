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
    bool jetpackIsOn;

    [Header("Time Travel Device")]
    public GameObject TimeDeviceHolder;
    public Image timeDeviceImage;
    public Sprite forward;
    public Sprite backward;
    bool ttdIsOn;
    bool inPresent;

    PlayerItemPickUper playerItemPickUper;
    PlayerMovement playerMovement;
    PlayerJetpack playerJetpack;
    PlayerCauseAndEffect playerCnE;

    Color defaultPointerColor;

    void Start()
    {
        playerItemPickUper = FindObjectOfType<PlayerItemPickUper>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerJetpack = FindObjectOfType<PlayerJetpack>();
        playerCnE = FindObjectOfType<PlayerCauseAndEffect>();

        itemName.text = "";
        defaultPointerColor = pointer.color;

        crouched = playerMovement.isCrouched;

        jetpackIsOn = playerJetpack.isOnAtStart;
        jetpackUIHolder.SetActive(jetpackIsOn);

        ttdIsOn = playerCnE.isOnAtStart;
        inPresent = playerCnE.inPresent;
        TimeDeviceHolder.SetActive(ttdIsOn);
    }

    void Update()
    {
        UpdatePointer();
        UpdateStanceUI();
        UpdateJetpackUI();
        UpdateTimeTravelDeviceUI();
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
        if (jetpackIsOn != playerJetpack.IsOn)
        {
            jetpackIsOn = playerJetpack.IsOn;
            jetpackUIHolder.SetActive(jetpackIsOn);
        }

        if (!jetpackIsOn) return;

        float fuelPercent = Mathf.Clamp01(playerJetpack.fuel / playerJetpack.maxFuel);
        jetpackFuelBarRect.localScale = new Vector3(fuelPercent, 1, 1);
        jetpackFuelBarImage.color = GetColorFromGradient(fuelPercent);
    }

    void UpdateTimeTravelDeviceUI()
    {
        if(ttdIsOn != playerCnE.isOn)
        {
            ttdIsOn = playerCnE.isOn;
            TimeDeviceHolder.SetActive(ttdIsOn);
        }

        if (!ttdIsOn) return;

        if(inPresent != playerCnE.inPresent)
        {
            inPresent = playerCnE.inPresent;
            if (inPresent)
            {
                timeDeviceImage.sprite = forward;
            }
            else
            {
                timeDeviceImage.sprite = backward;
            }
        }
    }

    Color GetColorFromGradient(float value)
    {
        return fuelGradient.Evaluate(value);
    }
}

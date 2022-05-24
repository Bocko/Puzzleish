using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Item Pick")]
    public Image pointer;
    public TextMeshProUGUI itemNameText;
    public Color highlightedPointerColor;
    Color defaultPointerColor;
    bool moveable;

    [Header("Stance Indicator")]
    public Image stanceHolder;
    public Sprite standingSprite;
    public Sprite crouchingSprite;
    bool crouched;

    [Header("Jetpack")]
    public RectTransform jetpackFuelBarRect;
    public Image jetpackFuelBarImage;
    public GameObject jetpackUIHolder;
    public Gradient fuelGradient;
    bool jetpackIsOn;
    float currentFuel;

    [Header("Time Travel Device")]
    public GameObject TimeDeviceHolder;
    public Image timeDeviceImage;
    public Sprite forward;
    public Sprite backward;
    bool ttdIsOn;
    bool inPresent;

    [Header("Item Thrower")]
    public ProgressBar throwBar;

    PlayerItemPickUper playerItemPickUper;
    PlayerMovement playerMovement;
    PlayerJetpack playerJetpack;
    PlayerCauseAndEffect playerCnE;

    void Start()
    {
        playerItemPickUper = FindObjectOfType<PlayerItemPickUper>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerJetpack = FindObjectOfType<PlayerJetpack>();
        playerCnE = FindObjectOfType<PlayerCauseAndEffect>();

        itemNameText.text = "";
        defaultPointerColor = pointer.color;
        moveable = playerItemPickUper.moveable;

        crouched = playerMovement.isCrouched;

        jetpackIsOn = playerJetpack.isOnAtStart;
        jetpackUIHolder.SetActive(jetpackIsOn);
        currentFuel = playerJetpack.fuel;

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
        UpdateThrowRadialBar();
    }

    void UpdatePointer()
    {
        if (moveable != playerItemPickUper.moveable)
        {
            moveable = playerItemPickUper.moveable;

            itemNameText.text = playerItemPickUper.currentItemName;
            pointer.color = moveable ? highlightedPointerColor : defaultPointerColor;
        }
    }

    void UpdateStanceUI()
    {
        if (crouched != playerMovement.isCrouched)
        {
            crouched = playerMovement.isCrouched;
            stanceHolder.sprite = crouched ? crouchingSprite : standingSprite;
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

        if (currentFuel != playerJetpack.fuel)
        {
            currentFuel = playerJetpack.fuel;
            float fuelPercent = Mathf.Clamp01(playerJetpack.fuel / playerJetpack.maxFuel);
            jetpackFuelBarRect.localScale = new Vector3(fuelPercent, 1, 1);
            jetpackFuelBarImage.color = GetColorFromGradient(fuelPercent);
        }
    }

    void UpdateTimeTravelDeviceUI()
    {
        if (ttdIsOn != playerCnE.IsOn)
        {
            ttdIsOn = playerCnE.IsOn;
            TimeDeviceHolder.SetActive(ttdIsOn);
        }

        if (!ttdIsOn) return;

        if (inPresent != playerCnE.inPresent)
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

    void UpdateThrowRadialBar()
    {
        if (playerItemPickUper.currentCharge >= 1)
        {
            return;
        }
        else if (playerItemPickUper.currentCharge > 0)
        {
            throwBar.barColor = GetColorFromGradient(playerItemPickUper.currentCharge);
            throwBar.ChangeCurrent(playerItemPickUper.currentCharge);
        }
        else if (throwBar.current != 0)
        {
            throwBar.ChangeCurrent(playerItemPickUper.currentCharge);
        }
    }

    Color GetColorFromGradient(float value)
    {
        return fuelGradient.Evaluate(value);
    }
}
